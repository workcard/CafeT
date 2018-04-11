namespace BusinessObjects
{
    using System;
    using System.Data.Entity;
    using Repository.Pattern.Ef6;
    using Crawler;


    public partial class CrawlersDbContext : DataContext
    {
        public CrawlersDbContext()
            : base("name=BusinessDbContext")
        {
        }

        public static CrawlersDbContext Create()
        {
            return new CrawlersDbContext();
        }
       

        public DbSet<Url> Urls { get; set; }
        //public DbSet<ArticleBo> Articles { get; set; }
        //public DbSet<CommentBo> Comments { get; set; }
        //public DbSet<ExamBo> Exams { get; set; }
        //public DbSet<QuestionBo> Questions { get; set; }
        //public DbSet<AnswerBo> Answers { get; set; }
        //public DbSet<ChoiceBo> Choices { get; set; }
        //public DbSet<FileBo> Files { get; set; }
        //public DbSet<ImageBo> Images { get; set; }
        //public DbSet<PrizeBo> Prizes { get; set; }

        //public DbSet<ArticleQuestion> ArticleQuestions { get; set; }
        //public DbSet<ExamQuestion> ExamQuestions { get; set; }
        //public DbSet<JobQuestion> JobQuestions { get; set; }
        //public DbSet<ExamPrize> ExamPrizes { get; set; }
        //public DbSet<ExamUser> ExamUsers { get; set; }
        //public DbSet<JobUser> JobUsers { get; set; }
        //public DbSet<CompanyBo> Companies { get; set; }
        //public DbSet<JobBo> Jobs { get; set; }
        //public DbSet<LevelBo> Levels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }
            
            //modelBuilder.Entity<ProductBo>().ToTable("Products");
            modelBuilder.Entity<Url>().ToTable("Urls");
            //modelBuilder.Entity<ArticleBo>().ToTable("Articles");
            //modelBuilder.Entity<CommentBo>().ToTable("Comments");
            //modelBuilder.Entity<ExamBo>().ToTable("Exams");
            //modelBuilder.Entity<QuestionBo>().ToTable("Questions");
            //modelBuilder.Entity<AnswerBo>().ToTable("Answers");
            //modelBuilder.Entity<ChoiceBo>().ToTable("Choices");
            //modelBuilder.Entity<FileBo>().ToTable("Files");
            //modelBuilder.Entity<ImageBo>().ToTable("Images");
            //modelBuilder.Entity<PrizeBo>().ToTable("Prizes");
            //modelBuilder.Entity<CompanyBo>().ToTable("Companies");
            //modelBuilder.Entity<JobBo>().ToTable("Jobs");
            //modelBuilder.Entity<LevelBo>().ToTable("Levels");
        }
    }
}
