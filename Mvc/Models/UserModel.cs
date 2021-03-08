using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarkelPlace.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public int RoleID { get; set; }
        [Required (AllowEmptyStrings =false,ErrorMessage ="First name Required")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name Required")]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Required")]
        public string EmailID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ConfirmPassword  Required")]
        [DataType(DataType.Password)]
        [Compare ("Password", ErrorMessage = "Password Did Not Match")]
        public string ConfirmPassword { get; set; }
        public bool IsEmailVerified { get; set; }
        public System.Guid Code { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }

        

    }
}