using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Domain.Entities
{
	public class ContactModel
	{
		[Required]
		public string Name { get; set; }
		public int UserId { get; set; }

		[Required, EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Subject { get; set; }

		[Required]
		public string Message { get; set; }
	}

}
