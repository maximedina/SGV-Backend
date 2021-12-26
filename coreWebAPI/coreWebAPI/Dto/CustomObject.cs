using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MOM.Core.Dto
{
	[NotMapped]
	public class CustomObject
	{
		public long Id { get; set; }
		public string Name { get; set; }
	}
}
