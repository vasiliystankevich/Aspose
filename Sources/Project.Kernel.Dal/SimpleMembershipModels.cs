using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Kernel.Dal
{
    [Table("Accounts")]
    public class AccountModel
    {
        public AccountModel()
        {
            UserId = Guid.NewGuid();
            AccountName = string.Empty;
            Email = string.Empty;
            Role = string.Empty;
            Note = string.Empty;
            IsActivate = false;
        }

        public Guid? UserId { get; set; }
        public bool? IsActivate { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Note { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public class webpages_Membership
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(128)]
        public string ConfirmationToken { get; set; }

        public bool? IsConfirmed { get; set; }

        public DateTime? LastPasswordFailureDate { get; set; }

        public int PasswordFailuresSinceLastSuccess { get; set; }

        [Required]
        [StringLength(128)]
        public string Password { get; set; }

        public DateTime? PasswordChangedDate { get; set; }

        [Required]
        [StringLength(128)]
        public string PasswordSalt { get; set; }

        [StringLength(128)]
        public string PasswordVerificationToken { get; set; }

        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }
    }

    public class webpages_OAuthMembership
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string Provider { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string ProviderUserId { get; set; }

        public int UserId { get; set; }
    }

    public class webpages_Roles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public webpages_Roles()
        {
            webpages_UsersInRoles = new HashSet<webpages_UsersInRoles>();
        }

        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(256)]
        public string RoleName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
    }

    public class webpages_UsersInRoles
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleId { get; set; }

        public virtual webpages_Roles webpages_Roles { get; set; }
    }
}
