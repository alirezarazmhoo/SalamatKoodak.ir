using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SalamatKoodak.Models
{
	public class Personels
	{
		public int Id { get; set; }
		[Required(ErrorMessage ="کد را وارد کنید")]
		public string Code { get; set; }
		[Required(ErrorMessage = "نام را وارد کنید")]
		public string Name { get; set; }

		[Required(ErrorMessage = "نام خانوادگی را وارد کنید")]

		public string LastName { get; set; }

		[Required(ErrorMessage = "موبایل را وارد کنید")]
		public string Mobile { get; set; }
		[Required(ErrorMessage = "ای دی تلگرام را وارد کنید")]

		public string IdTelegram { get; set; }
		public string NationalCode { get; set; }
		[Required]
		public int RelationTypeId { get; set; }
		public RelationType RelationType { get; set; }
		[Required]

		public int PersonelStatusId { get; set; }
		public PersonelStatus PersonelStatus { get; set; }
		public int CityId { get; set; }
		public City City { get; set; }
		public string ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }

	}
}