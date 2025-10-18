using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMithila.SharedDataAccess.EFCore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    ConversationId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsGroup = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())"),
                    CreatedBy = table.Column<int>(nullable: true),
                    LastMessageOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Conversa__C050D877DB208581", x => x.ConversationId);
                });

            migrationBuilder.CreateTable(
                name: "Hashtags",
                columns: table => new
                {
                    HashtagId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tag = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Hashtags__BEFA912A84475444", x => x.HashtagId);
                });

            migrationBuilder.CreateTable(
                name: "SearchIndex",
                columns: table => new
                {
                    SearchId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<string>(maxLength: 50, nullable: true),
                    EntityId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SearchIn__21C535F484ACCA41", x => x.SearchId);
                });

            migrationBuilder.CreateTable(
                name: "TblUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(maxLength: 150, nullable: false),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    Country = table.Column<string>(maxLength: 100, nullable: true),
                    Address = table.Column<string>(maxLength: 250, nullable: true),
                    TownCity = table.Column<string>(maxLength: 100, nullable: true),
                    PostCode = table.Column<string>(maxLength: 10, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProfilePhoto = table.Column<string>(maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(nullable: true, defaultValueSql: "((1))"),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    LastActiveOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFeed",
                columns: table => new
                {
                    FeedId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    PostId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())"),
                    IsRead = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserFeed__1586DF5566872EAB", x => x.FeedId);
                });

            migrationBuilder.CreateTable(
                name: "ContentReports",
                columns: table => new
                {
                    ReportId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReporterId = table.Column<int>(nullable: false),
                    EntityType = table.Column<string>(maxLength: 50, nullable: false),
                    EntityId = table.Column<long>(nullable: false),
                    Reason = table.Column<string>(maxLength: 500, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())"),
                    IsResolved = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ContentR__D5BD4805ECBF10AE", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Report_Reporter",
                        column: x => x.ReporterId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConversationMembers",
                columns: table => new
                {
                    ConversationMemberId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversationId = table.Column<long>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    JoinedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())"),
                    IsMuted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Conversa__6CF98427BE7F3769", x => x.ConversationMemberId);
                    table.ForeignKey(
                        name: "FK_CM_Conversation",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "ConversationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CM_User",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    FriendshipId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequesterId = table.Column<int>(nullable: false),
                    AddresseeId = table.Column<int>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Friendsh__4D531A542EFD9126", x => x.FriendshipId);
                    table.ForeignKey(
                        name: "FK_Friend_Addressee",
                        column: x => x.AddresseeId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friend_Requester",
                        column: x => x.RequesterId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())"),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Groups__149AF36A1A68D5C9", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Group_Creator",
                        column: x => x.CreatedBy,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    MediaId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerUserId = table.Column<int>(nullable: true),
                    MediaUrl = table.Column<string>(maxLength: 500, nullable: false),
                    MediaType = table.Column<string>(maxLength: 30, nullable: false),
                    FileName = table.Column<string>(maxLength: 260, nullable: true),
                    SizeBytes = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MediaFil__B2C2B5CF68DED6AF", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_Media_Owner",
                        column: x => x.OwnerUserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversationId = table.Column<long>(nullable: false),
                    SenderId = table.Column<int>(nullable: false),
                    ContentText = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())"),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    EditedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Messages__C87C0C9CA01E0C1F", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Message_Conversation",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "ConversationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_Sender",
                        column: x => x.SenderId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ActorId = table.Column<int>(nullable: true),
                    NotificationType = table.Column<string>(maxLength: 50, nullable: false),
                    EntityType = table.Column<string>(maxLength: 50, nullable: true),
                    EntityId = table.Column<long>(nullable: true),
                    IsRead = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__20CF2E1291BE5B8F", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_Actor",
                        column: x => x.ActorId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notification_User",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ContentText = table.Column<string>(nullable: true),
                    Privacy = table.Column<byte>(nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())"),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    Latitude = table.Column<decimal>(type: "decimal(9, 6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(9, 6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Posts__AA1260181FCF6913", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Post_User",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RateLimits",
                columns: table => new
                {
                    RateLimitId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: true),
                    ActionType = table.Column<string>(maxLength: 50, nullable: true),
                    Count = table.Column<int>(nullable: true, defaultValueSql: "((0))"),
                    WindowStart = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    WindowEnd = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RateLimi__0B581845C15C954D", x => x.RateLimitId);
                    table.ForeignKey(
                        name: "FK__RateLimit__UserI__30C33EC3",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    StoryId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    MediaUrl = table.Column<string>(maxLength: 500, nullable: false),
                    MediaType = table.Column<string>(maxLength: 30, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())"),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    Latitude = table.Column<decimal>(type: "decimal(9, 6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(9, 6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Stories__3E82C048965EC5E3", x => x.StoryId);
                    table.ForeignKey(
                        name: "FK_Story_User",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblAddress",
                columns: table => new
                {
                    AddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    Address = table.Column<string>(maxLength: 250, nullable: false),
                    PinCode = table.Column<string>(maxLength: 10, nullable: true),
                    GoogleMapLocation = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true, defaultValueSql: "((1))"),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TblAddre__091C2AFBB2FE51AF", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK__TblAddres__UserI__3C69FB99",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tble_UserPassword",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    OldPassword = table.Column<string>(maxLength: 255, nullable: false),
                    NewPassword = table.Column<string>(maxLength: 255, nullable: false),
                    IsBlocked = table.Column<bool>(nullable: false),
                    AddedOn = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tble_UserPassword", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tble_UserPassword_Users",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblSocial",
                columns: table => new
                {
                    SocialId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    FacebookProfile = table.Column<string>(maxLength: 200, nullable: true),
                    Twitter = table.Column<string>(maxLength: 200, nullable: true),
                    LinkedIn = table.Column<string>(maxLength: 200, nullable: true),
                    Instagram = table.Column<string>(maxLength: 200, nullable: true),
                    Flickr = table.Column<string>(maxLength: 200, nullable: true),
                    Github = table.Column<string>(maxLength: 200, nullable: true),
                    Skype = table.Column<string>(maxLength: 200, nullable: true),
                    Google = table.Column<string>(maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(nullable: true, defaultValueSql: "((1))"),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TblSocia__67CF711A78A170B5", x => x.SocialId);
                    table.ForeignKey(
                        name: "FK__TblSocial__UserI__4222D4EF",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblUserPaymentCard",
                columns: table => new
                {
                    PaymentCardId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<string>(maxLength: 20, nullable: false),
                    HolderName = table.Column<string>(maxLength: 100, nullable: false),
                    ExpiryMonth = table.Column<int>(nullable: false),
                    ExpiryYear = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true, defaultValueSql: "((1))"),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TblUserP__833403D82358948C", x => x.PaymentCardId);
                    table.ForeignKey(
                        name: "FK__TblUserPa__UserI__47DBAE45",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupMembers",
                columns: table => new
                {
                    GroupMemberId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<long>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Role = table.Column<byte>(nullable: false),
                    JoinedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GroupMem__344812925772D5FA", x => x.GroupMemberId);
                    table.ForeignKey(
                        name: "FK_GM_Group",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GM_User",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageAttachments",
                columns: table => new
                {
                    AttachmentId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<long>(nullable: false),
                    MediaUrl = table.Column<string>(maxLength: 500, nullable: false),
                    MediaType = table.Column<string>(maxLength: 30, nullable: false),
                    SizeBytes = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MessageA__442C64BE77F1800F", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_MA_Message",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageReceipts",
                columns: table => new
                {
                    ReceiptId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<long>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    DeliveryStatus = table.Column<byte>(nullable: false),
                    StatusOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MessageR__CC08C4206288BD81", x => x.ReceiptId);
                    table.ForeignKey(
                        name: "FK_Receipt_Message",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Receipt_User",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<long>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ParentCommentId = table.Column<long>(nullable: true),
                    CommentText = table.Column<string>(maxLength: 1000, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())"),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comments__C3B4DFCA7B055521", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comment_Post",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_User",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostMedia",
                columns: table => new
                {
                    PostMediaId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<long>(nullable: false),
                    MediaUrl = table.Column<string>(maxLength: 500, nullable: false),
                    MediaType = table.Column<string>(maxLength: 30, nullable: false),
                    SortOrder = table.Column<byte>(nullable: true, defaultValueSql: "((0))"),
                    MetadataJson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostMedia", x => x.PostMediaId);
                    table.ForeignKey(
                        name: "FK_PostMedia_Post",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTags",
                columns: table => new
                {
                    PostTagId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<long>(nullable: true),
                    HashtagId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PostTags__325724FD06B3B301", x => x.PostTagId);
                    table.ForeignKey(
                        name: "FK__PostTags__Hashta__42E1EEFE",
                        column: x => x.HashtagId,
                        principalTable: "Hashtags",
                        principalColumn: "HashtagId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__PostTags__PostId__41EDCAC5",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    ReactionId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    PostId = table.Column<long>(nullable: true),
                    CommentId = table.Column<long>(nullable: true),
                    ReactionType = table.Column<string>(maxLength: 30, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(3)", nullable: true, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reaction__46DDF9B4933872DA", x => x.ReactionId);
                    table.ForeignKey(
                        name: "FK_Reaction_Comment",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reaction_Post",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reaction_User",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserMentions",
                columns: table => new
                {
                    MentionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<long>(nullable: true),
                    CommentId = table.Column<long>(nullable: true),
                    MentionedUserId = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserMent__5D9162DCFDD9DD8D", x => x.MentionId);
                    table.ForeignKey(
                        name: "FK__UserMenti__Comme__3D2915A8",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserMenti__Menti__3E1D39E1",
                        column: x => x.MentionedUserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UserMenti__PostI__3C34F16F",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId_CreatedOn",
                table: "Comments",
                columns: new[] { "PostId", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_ContentReports_ReporterId",
                table: "ContentReports",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMembers_UserId",
                table: "ConversationMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMembers_Conversation_User",
                table: "ConversationMembers",
                columns: new[] { "ConversationId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_AddresseeId",
                table: "Friendships",
                column: "AddresseeId");

            migrationBuilder.CreateIndex(
                name: "UX_Friendship_Requester_Addressee",
                table: "Friendships",
                columns: new[] { "RequesterId", "AddresseeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_UserId",
                table: "GroupMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UX_GroupMembers_Group_User",
                table: "GroupMembers",
                columns: new[] { "GroupId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CreatedBy",
                table: "Groups",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ__Hashtags__C4516413AC41CE82",
                table: "Hashtags",
                column: "Tag",
                unique: true,
                filter: "[Tag] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_Owner",
                table: "MediaFiles",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageAttachments_MessageId",
                table: "MessageAttachments",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReceipts_UserId",
                table: "MessageReceipts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReceipts_Message_User",
                table: "MessageReceipts",
                columns: new[] { "MessageId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId_CreatedOn",
                table: "Messages",
                columns: new[] { "ConversationId", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ActorId",
                table: "Notifications",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_User_IsRead",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_PostMedia_PostId",
                table: "PostMedia",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId_CreatedOn",
                table: "Posts",
                columns: new[] { "UserId", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_HashtagId",
                table: "PostTags",
                column: "HashtagId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_PostId",
                table: "PostTags",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_RateLimits_UserId",
                table: "RateLimits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CommentId",
                table: "Reactions",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_PostId",
                table: "Reactions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "UX_Reaction_User_Target",
                table: "Reactions",
                columns: new[] { "UserId", "PostId", "CommentId" },
                unique: true,
                filter: "[PostId] IS NOT NULL AND [CommentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_UserId_ExpiresOn",
                table: "Stories",
                columns: new[] { "UserId", "ExpiresOn" });

            migrationBuilder.CreateIndex(
                name: "IX_TblAddress_UserId",
                table: "TblAddress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tble_UserPassword_UserId",
                table: "Tble_UserPassword",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TblSocial_UserId",
                table: "TblSocial",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__TblUser__A9D10534C2F22E84",
                table: "TblUser",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TblUser_LastActiveOn",
                table: "TblUser",
                column: "LastActiveOn");

            migrationBuilder.CreateIndex(
                name: "IX_TblUserPaymentCard_UserId",
                table: "TblUserPaymentCard",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFeed_UserId_CreatedOn",
                table: "UserFeed",
                columns: new[] { "UserId", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_UserMentions_CommentId",
                table: "UserMentions",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMentions_MentionedUserId",
                table: "UserMentions",
                column: "MentionedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMentions_PostId",
                table: "UserMentions",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentReports");

            migrationBuilder.DropTable(
                name: "ConversationMembers");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "GroupMembers");

            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropTable(
                name: "MessageAttachments");

            migrationBuilder.DropTable(
                name: "MessageReceipts");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PostMedia");

            migrationBuilder.DropTable(
                name: "PostTags");

            migrationBuilder.DropTable(
                name: "RateLimits");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "SearchIndex");

            migrationBuilder.DropTable(
                name: "Stories");

            migrationBuilder.DropTable(
                name: "TblAddress");

            migrationBuilder.DropTable(
                name: "Tble_UserPassword");

            migrationBuilder.DropTable(
                name: "TblSocial");

            migrationBuilder.DropTable(
                name: "TblUserPaymentCard");

            migrationBuilder.DropTable(
                name: "UserFeed");

            migrationBuilder.DropTable(
                name: "UserMentions");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Hashtags");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "TblUser");
        }
    }
}
