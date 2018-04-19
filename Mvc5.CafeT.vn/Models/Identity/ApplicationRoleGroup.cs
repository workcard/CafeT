namespace Mvc5.CafeT.vn.Models
{
    public class ApplicationRoleGroup
    {
        public virtual string RoleId { get; set; }
        public virtual int GroupId { get; set; }

        public virtual ApplicationRole Role { get; set; }
        public virtual ApplicationGroup Group { get; set; }
    }
}
