using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using MOM.Core.Models.Security;
using MOM.Core.Db;
using System.DirectoryServices.AccountManagement;
using System.Security.Cryptography;
using System.Text;
using MOM.Core.WebAPI.Models;

namespace MOM.Core.Services
{
    public class SecurityService : IHostedService, IDisposable
    {
        private const int KILL_SESSION_INTERVAL = 60; //In seconds
        private const double EXPIRATION_LIMIT = 1440; //1440; //In minutes
        private Timer timer;
        private static readonly Object workLock = new Object();
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger logger;
        private Dictionary<string, Session> currentSessions;
        private readonly IConfiguration configuration;

        public SecurityService(IServiceScopeFactory scopeFactory, ILogger<SecurityService> logger, IConfiguration configuration)
        {
            currentSessions = new Dictionary<string, Session>();

            this.scopeFactory = scopeFactory;
            this.logger = logger;
            this.configuration = configuration;
            LoadSessions();
        }

        private void LoadSessions()
        {
            using (var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<CoreDbContext>())
            {
                dbContext.Session.ToList().ForEach(s => currentSessions.TryAdd(s.Token, s));
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(KILL_SESSION_INTERVAL));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            CheckSessions();
        }

        public Session LoginOnFrame(string login, string ipAddress, string checkSum, string deviceType)
        {
            string loginDecrypted = string.Empty;
            string checkSumToValidate = string.Empty;

            using (SHA1 sha1Hash = SHA1.Create())
            {
                //From String to byte array
                byte[] sourceBytes = Encoding.UTF8.GetBytes(login + ipAddress);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                if (hash != null)
                    checkSumToValidate = hash;
            }

            if (checkSumToValidate != checkSum)
                throw new Exception("ERROR iniciando Single Sign On. Inicie sesión nuevamente en el Portal MOM.");
            

            using (var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<CoreDbContext>())
            {
                var systemSetting = new SystemSetting(dbContext);
                var encryptKey = systemSetting.GetByName("ENCRYPT_KEY").Value;

                if (string.IsNullOrEmpty(encryptKey))
                    throw new Exception("ERROR iniciando Single Sign On. No es posible recuperar clave [ENCRYPT_KEY].");

                loginDecrypted = Crypto.Decrypt(login, encryptKey);

                Session session = null;
                
                var dbUser = dbContext.Users.Include(x => x.Profile).Where(x => x.Login.ToUpper().Equals(loginDecrypted.ToUpper())).FirstOrDefault();
                if (dbUser != null)
                {
                    lock (workLock)
                    {
                        session = new Session()
                        {
                            CreationTime = DateTime.Now,
                            DeviceType = deviceType,
                            Token = Guid.NewGuid().ToString(),
                            LastAction = DateTime.Now,
                            User = dbUser,
                        };

                        dbContext.Session.Add(session);
                        dbContext.SaveChanges();

                        currentSessions.Add(session.Token, session);

                        session.Permissions = dbContext.ProfilePermissions
                        .Include(p => p.Permission)
                        .Where(pp => pp.Profile == session.User.Profile)
                        .Select(p => p.Permission).ToList();
                    }
                }
                
                return session;
            }
        }

        public Session Login(string login, string password, string deviceType)
        {
            using (var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<CoreDbContext>())
            {
                Session session = null;
                bool loggedAd = false;

                //// auth local              
                var dbUser = dbContext.Users.Include(x => x.Profile).Where(x => x.Login.ToUpper().Equals(login.ToUpper())).FirstOrDefault();
                if (dbUser != null)
                {
                    if (dbUser.Password == password)
                    { loggedAd = true; }
                }
                /////

                if (loggedAd)
                {
                    //var dbUser = dbContext.Users.Include(x => x.Profile).Where(x => x.Login.ToUpper().Equals(login.ToUpper())).FirstOrDefault();

                    if (dbUser != null)
                    {
                        lock (workLock)
                        {
                            session = new Session()
                            {
                                CreationTime = DateTime.Now,
                                DeviceType = deviceType,
                                Token = Guid.NewGuid().ToString(),
                                LastAction = DateTime.Now,
                                User = dbUser,
                            };

                            dbContext.Session.Add(session);
                            dbContext.SaveChanges();

                            currentSessions.Add(session.Token, session);

                            session.Permissions = dbContext.ProfilePermissions
                            .Include(p => p.Permission)
                            .Where(pp => pp.Profile == session.User.Profile)
                            .Select(p => p.Permission).ToList();
                        }
                    }
                }

                return session;
            }
        }

        public void Logout(string token)
        {
            lock (workLock)
            {
                if (currentSessions.ContainsKey(token))
                {
                    Logout(currentSessions[token]);
                }
            }
        }

        public Session GetCurrentSession(HttpRequest request)
        {
            using (var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<CoreDbContext>())
            {
                return dbContext.Session.Where((x) => x.Token == request.Headers["sessionToken"][0]).FirstOrDefault();
            }
        }

        private void Logout(Session session)
        {
            // TODO: review Logout method urgently. OVERHEAD
            try
            {
                lock (workLock)
                {
                    var sessionLog = (SessionLog)session;
                    currentSessions.Remove(session.Token);

                    using (var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<CoreDbContext>())
                    {
                        dbContext.Entry(sessionLog).State = EntityState.Added;
                        dbContext.SessionLog.Add(sessionLog);

                        if (dbContext.Session.Any(x => x.Id == session.Id))
                        {
                            dbContext.Entry(session).State = EntityState.Deleted;
                            dbContext.Session.Remove(session);
                        }
                            
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Logout throwed exception. Database tried to delete one row and it throwed a warning.");
            }
        }


        private void CheckSessions()
        {
            var expiredSessions = new List<Session>();
            lock (workLock)
            {
                currentSessions.Values.ToList().ForEach(v =>
                {
                    if (v.LastAction < DateTime.Now.AddMinutes(EXPIRATION_LIMIT * -1))
                    {
                        expiredSessions.Add(v);
                    }
                });
                expiredSessions.ForEach(e => Logout(e));
            }
        }

        public bool HasSession(string token)
        {
            if (currentSessions.ContainsKey(token))
            {
                UpdateActivity(currentSessions[token]);
                return true;
            }
            else // TODO: Retirar loadSession, alternativa para funcionar
            {
                currentSessions.Clear();
                LoadSessions();
                if (currentSessions.ContainsKey(token))
                {
                    UpdateActivity(currentSessions[token]);
                    return true;
                }
            }
            return false;
        }

        public async void UpdateActivity(Session session)
        {
            using (var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<CoreDbContext>())
            {
                if (dbContext.Session.Any(x => x.Id == session.Id))
                {
                    session.LastAction = DateTime.Now;
                    dbContext.Entry(session).State = EntityState.Added;
                    dbContext.Session.Update(session);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public Session GetSession(string token)
        {
            if (currentSessions.ContainsKey(token))
            {
                return currentSessions[token];
            }
            return null;
        }
    }
}
