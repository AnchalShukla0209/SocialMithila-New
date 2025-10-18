using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SocialMithila.SharedDataAccess.EFCore
{
    public partial class TblUser
    {
        public TblUser()
        {
            Comments = new HashSet<Comments>();
            ContentReports = new HashSet<ContentReports>();
            ConversationMembers = new HashSet<ConversationMembers>();
            FriendshipsAddressee = new HashSet<Friendships>();
            FriendshipsRequester = new HashSet<Friendships>();
            GroupMembers = new HashSet<GroupMembers>();
            Groups = new HashSet<Groups>();
            MediaFiles = new HashSet<MediaFiles>();
            MessageReceipts = new HashSet<MessageReceipts>();
            Messages = new HashSet<Messages>();
            NotificationsActor = new HashSet<Notifications>();
            NotificationsUser = new HashSet<Notifications>();
            Posts = new HashSet<Posts>();
            RateLimits = new HashSet<RateLimits>();
            Reactions = new HashSet<Reactions>();
            Stories = new HashSet<Stories>();
            TblAddress = new HashSet<TblAddress>();
            TblSocial = new HashSet<TblSocial>();
            TblUserPaymentCard = new HashSet<TblUserPaymentCard>();
            TbleUserPassword = new HashSet<TbleUserPassword>();
            UserMentions = new HashSet<UserMentions>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        [StringLength(150)]
        public string Email { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        [StringLength(100)]
        public string Country { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
        [StringLength(100)]
        public string TownCity { get; set; }
        [StringLength(10)]
        public string PostCode { get; set; }
        public string Description { get; set; }
        [StringLength(255)]
        public string ProfilePhoto { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        [StringLength(100)]
        public string ModifiedBy { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? LastActiveOn { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Comments> Comments { get; set; }
        [InverseProperty("Reporter")]
        public virtual ICollection<ContentReports> ContentReports { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ConversationMembers> ConversationMembers { get; set; }
        [InverseProperty(nameof(Friendships.Addressee))]
        public virtual ICollection<Friendships> FriendshipsAddressee { get; set; }
        [InverseProperty(nameof(Friendships.Requester))]
        public virtual ICollection<Friendships> FriendshipsRequester { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<GroupMembers> GroupMembers { get; set; }
        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<Groups> Groups { get; set; }
        [InverseProperty("OwnerUser")]
        public virtual ICollection<MediaFiles> MediaFiles { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<MessageReceipts> MessageReceipts { get; set; }
        [InverseProperty("Sender")]
        public virtual ICollection<Messages> Messages { get; set; }
        [InverseProperty(nameof(Notifications.Actor))]
        public virtual ICollection<Notifications> NotificationsActor { get; set; }
        [InverseProperty(nameof(Notifications.User))]
        public virtual ICollection<Notifications> NotificationsUser { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Posts> Posts { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<RateLimits> RateLimits { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Reactions> Reactions { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Stories> Stories { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TblAddress> TblAddress { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TblSocial> TblSocial { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TblUserPaymentCard> TblUserPaymentCard { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TbleUserPassword> TbleUserPassword { get; set; }
        [InverseProperty("MentionedUser")]
        public virtual ICollection<UserMentions> UserMentions { get; set; }
    }
}
