using CafeT.Html;
using CafeT.Objects.Enums;
using CafeT.Text;
using CafeT.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Models;

namespace Web.ModelViews
{
    public class IssueInfo
    {
        public Project Project { set; get; }
        public IEnumerable<Question> Questions { set; get; }
        public IEnumerable<Comment> Comments { set; get; }
    }

    public class IssueView : BaseObject
    {
        public string Title { set; get; }
        public string Description { get; set; }
        public string Content { set; get; }
        public string Message { set; get; } = string.Empty;
        public string Owner { set; get; }
        public double Price { set; get; } = 0;
        public string AssignedUserName { set; get; } = string.Empty;

        public DateTime? Start { set; get; }
        public DateTime? End { set; get; }
        public decimal IssueEstimation { set; get; } = 15; //Default by 30 minutes

        public IssueStatus Status { set; get; } = IssueStatus.New;
        public ScheduleType Repeat { set; get; } = ScheduleType.None;

        public Guid? ProjectId { set; get; }
        public string[] Keywords { set; get; }
        public virtual IEnumerable<Comment> Comments { set; get; }
        public virtual IEnumerable<Question> Questions { set; get; }

        public List<string> Tags { set; get; }
        public List<string> Numbers { set; get; }
        public List<DateTime> Times { set; get; }
        public List<string> Emails { set; get; }
        public List<string> HasTags { set; get; }
        public List<string> Members { set; get; }
        public List<string> Viewers { set; get; } = new List<string>();
        public bool IsClosed { set; get; }
        public bool IsVerified { set; get; }

        public IssueView():base()
        {
            HasTags = new List<string>();
            Members = new List<string>();
            Tags = new List<string>();
            Emails = new List<string>();
            Times = new List<DateTime>();
        }

        public bool IsOf(string userName)
        {
            if ((!this.CreatedBy.IsNullOrEmptyOrWhiteSpace() && (this.CreatedBy.ToLower() == userName))
                || (!this.Owner.IsNullOrEmptyOrWhiteSpace() && (this.Owner.ToLower() == userName))) return true;
            return false;
        }
        public bool IsFree()
        {
            if (this.Price == 0) return true;
            return false;
        }
        public bool IsStandard()
        {
            bool _isStandard = this.Start.HasValue && this.End.HasValue;
            return _isStandard;
        }
        public bool IsExpired()
        {
           if(this.Status != IssueStatus.Done)
            {
                if (this.End.HasValue && this.End.Value.IsExpired()) return true;
            }
            return false;
        }
        public bool IsUpcoming(int? n)
        {
            if(n.HasValue)
            {
                if (End.HasValue && !End.Value.IsExpired() 
                    && (End.Value > DateTime.Today.AddDays(n.Value)))
                    return true;
                return false;
            }
            else
            {
                if (End.HasValue && !End.Value.IsExpired() 
                    && (End.Value > DateTime.Now))
                    return true;
                return false;
            }
        }
        public bool IsNoTime()
        {
            if (End == null || !End.HasValue) return true;
            return false;
        }
        public bool IsToday()
        {
            if (End != null && End.HasValue && End.Value.IsToday()) return true;
            return false;
        }
        public bool IsInDay(DateTime date)
        {
            if (End != null && End.HasValue && (End.Value.Day == date.Day)) return true;
            return false;
        }
        public bool IsCompleted()
        {
            if (End.HasValue && this.Status == IssueStatus.Done) return true;
            return false;
        }
        public bool IsWeekend()
        {
            if (End != null && End.HasValue && End.Value.IsToday()) return true;
            return false;
        }
        public bool IsTomorrow()
        {
            if (End.HasValue && End.Value.IsTomorrow())
                return true;
            return false;
        }
        public bool IsNextDay(int n)
        {
            if (End.HasValue && End.Value.IsNextDay(n))
                return true;
            return false;
        }
        
        public bool IsValid()
        {
            if (Title.IsNullOrEmptyOrWhiteSpace()) return false;
            if (Content.IsNullOrEmptyOrWhiteSpace()) return false;
            return true;
        }
        public bool HasImage()
        {
            if (!this.IsValid()) return false;
            var images = Content.GetImages().Count();
            if (images > 0) return true;
            return false;
        }
        public bool IsPrevDay(int n)
        {
            if (End.HasValue && End.Value.Date.AddDays(n) <= DateTime.Now.Date)
                return true;
            return false;
        }
        public bool IsYesterday()
        {
            if (End.HasValue && End.Value.IsYesterday())
                return true;
            return false;
        }
        
        public bool IsRepeat()
        {
            if(this.Repeat == ScheduleType.None)
                return false;
            return true;
        }

        public void AutoSetTimes()
        {
            //Autofill time
            DateTime[] _times = Content.GetTimes();
            if (_times != null && _times.Count() > 0)
            {
                foreach (var _time in _times)
                {
                    Times.Add(_time);
                }
            }

            string[] _contains = Content.ToWords().Where(t => t.CountOf("/") == 1).ToArray();
            if(_contains != null && _contains.Length > 0)
            {
                foreach (string value in _contains)
                {
                    try
                    {
                        int year = DateTime.Now.Year;
                        int month = int.Parse(value.GetAfter("/"));
                        int day = int.Parse(value.GetBefore("/"));
                        var time = new DateTime(year, month, day) { };
                        Times.Add(time);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            //BuildTime();
            if (this.IsNoTime())
            {
                if (Times == null ||Times.Count == 0)
                {
                    this.SetNow();
                }
                else
                {
                    this.Start = Times.FirstOrDefault();
                    this.End = Times.LastOrDefault();
                }
            }
        }

        public bool HasKeywords(string keyWord)
        {
            if (keyWord == null || keyWord.IsNullOrEmptyOrWhiteSpace()) return false;
            keyWord = keyWord.ToLower();
            if (this.Content.IsNullOrEmptyOrWhiteSpace() 
                //|| this.Description.IsNullOrEmptyOrWhiteSpace() || this.Title.IsNullOrEmptyOrWhiteSpace()
                ) return false;
            if (this.Content.ToLower().Contains(keyWord)
                //|| this.Description.ToLower().Contains(keyWord)
                //|| this.Title.ToLower().Contains(keyWord))
                )
            {
                return true;
            }
            return false;
        }

        public void AutoAdjust()
        {
            if(Title.IsNullOrEmptyOrWhiteSpace())
            {
                HtmlToText convert = new HtmlToText();
                string _text = convert.Convert(this.Content);
                Title = _text.GetSentences().FirstOrDefault();
            }
            
            if(Title.ToLower().Contains("[daily]"))
            {
                Repeat = ScheduleType.Daily;
            }
            if (Title.ToLower().Contains("[weekly]"))
            {
                Repeat = ScheduleType.Weekly;
            }
            if (Title.ToLower().Contains("[monthly]"))
            {
                Repeat = ScheduleType.Monthly;
            }
            if (Title.ToLower().Contains("[quaterly]"))
            {
                Repeat = ScheduleType.Quaterly;
            }
            if (Title.ToLower().Contains("[yearly]"))
            {
                Repeat = ScheduleType.Yearly;
            }

            //AutoSetTime
            this.AutoSetTimes();

            //HasTags
            string[] _hasTags = Content.GetHasTags();
            if (_hasTags != null && _hasTags.Count() > 0)
            {
                foreach (string _hasTag in _hasTags)
                {
                    HasTags.Add(_hasTag);
                }
            }
            //Links
            //Links = this.Content.GetUrls().ToList();
        }

        public bool HasInnerMembers()
        {
            if (GetInnerMembers() != null && GetInnerMembers().Count() > 0) return true;
            return false;
        }

        public string[] GetInnerMembers()
        {
            List<string> _innerMembers = new List<string>();
            string[] _emails = Content.GetEmails();
            if (_emails != null && _emails.Count() > 0)
            {
                foreach (string _email in _emails)
                {
                    _innerMembers.Add(_email);
                }
            }
            return _innerMembers.ToArray();
        }

        public void SetToday()
        {
            this.Start = DateTime.Today.StartWorkingTime();
            this.End = DateTime.Today.StartWorkingTime().AddMinutes(30);
        }

        public void SetNow()
        {
            this.Start = DateTime.Now;
            this.End = DateTime.Now.AddMinutes(30);
        }

        public void SetTomorrow()
        {
            this.Start = DateTime.Today.AddDays(1).StartWorkingTime();
            this.End = DateTime.Today.AddDays(1).StartWorkingTime().AddMinutes(30);
        }

        public string GetMessage()
        {
            Message = string.Empty;

            if (!this.Start.HasValue)
            {
                Message += "\n Must setup [Start]";
            }

            if (!this.End.HasValue)
            {
                Message += "\n Must setup [End]";
            }
            if (this.IsNoTime())
            {
                Message += "\n Bạn chưa thiết lập thời gian thực thi. Vui lòng cập nhật để chúng tôi có thể hỗ trợ bạn hiệu quả hơn";
            }
            if (this.Status != IssueStatus.Done)
            {
                if (this.End.HasValue && this.End.Value.IsPrevDay(0))
                    Message += "\n Đã quá hạn " + this.End.Value.TimeAgo()  + ". Vui lòng cập nhật tiến độ";
            }
            return Message;
        }
        public string[] GetEmails()
        {
            List<string> _emails = new List<string>();
            if (Title.HasEmail()) _emails = Title.GetEmails().ToList();
            if (Content.HasEmail()) _emails.AddRange(Content.GetEmails().ToList());
            return _emails.ToArray();
        }

        public string[] GetCommands()
        {
            List<string> commands = new List<string>();
            string[] _patterns = new string[] { "{,}","[,]","$$,$$"};
            foreach(string _pattern in _patterns)
            {
                string open = _pattern.Split(new string[] { "," }, StringSplitOptions.None)[0];
                string close = _pattern.Split(new string[] { "," }, StringSplitOptions.None)[1];
                var _commands = Content.ExtractAllBetween(open, close);
                if(_commands != null && _commands.Count()>0)
                {
                    commands.AddRange(_commands.ToList());
                }
            }

            return commands.ToArray();
        }

        public bool IsRunning()
        {
            if (Start.HasValue && End.HasValue && DateTime.Now.IsInRange(Start.Value, End.Value))
            {
                return true;
            }
            return false;
        }
    }
}