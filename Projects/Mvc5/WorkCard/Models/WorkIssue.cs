using CafeT.Objects.Enums;
using CafeT.Text;
using CafeT.Time;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Models
{
    public class WorkIssue:BaseObject
    {
        public string Title { set; get; }
        public string Description { get; set; }
        public string Content { set; get; }
        public string Message { set; get; } = string.Empty;
        public string Owner { set; get; }
        public double Price { set; get; } = 0;
        public string AssignedUserName { set; get; } = string.Empty;


        public DateTime? Start { set; get; } = DateTime.Now;
        public DateTime? End { set; get; } = DateTime.Now.AddDays(1).StartWorkingTime();

        public double IssueEstimation { set; get; } = 10; //Default by 10 minutes
        public double Salary { set; get; } = double.Parse("0.22")* double.Parse("22,800.00"); //Salary by minutes

        public IssueStatus Status { set; get; } = IssueStatus.New;
        public ScheduleType Repeat { set; get; } = ScheduleType.None;

        public Guid? ProjectId { set; get; }
        public Guid? JobId { set; get; }

        public List<string> Tags { set; get; }
        public List<string> Numbers { set; get; }
        public List<DateTime> Times { set; get; }
        public List<string> Emails { set; get; }
        
        public List<string> HasTags { set; get; }
        public List<string> Members { set; get; }
        public List<string> Viewers { set; get; } = new List<string>();
        public bool IsClosed { set; get; }
        public bool IsVerified { set; get; } = false;
        
        public WorkIssue():base()
        {
            HasTags = new List<string>();
            Members = new List<string>();
            Tags = new List<string>();
            Emails = new List<string>();
            Times = new List<DateTime>();
        }
        

        public void Update()
        {
            UpdatePrice();
            UpdateMeta();
            UpdatedDate = DateTime.Now;
        }

        public void UpdateTime(DateTime time)
        {
            MoveTo(time);
        }

        public WorkIssue MoveTo(DateTime time)
        {
            Start = time;
            End = Start.Value.AddMinutes(IssueEstimation);
            return this;
        }

        public void UpdatePrice()
        {
            Price = IssueEstimation * Salary;
        }

        public void UpdateMeta()
        {
            Tags.Add(Title.GetFromTo("[", "]"));
        }

        public override bool IsOf(string userName)
        {
            if (base.IsOf(userName) || (!Owner.IsNullOrEmptyOrWhiteSpace() && (Owner.ToLower() == userName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddViewer(string userName)
        {
            Viewers.Add(userName);
        }

        public bool IsFree()
        {
            if (Price == 0) return true;
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
                if (End.HasValue && End.Value.IsExpired()) return true;
            }
            return false;
        }

        public bool IsUpcoming(int? n)
        {
            if(n.HasValue)
            {
                if (End.HasValue && !End.Value.IsExpired() && (End.Value > DateTime.Today.AddDays(n.Value))) return true;
                return false;
            }
            else
            {
                if (End.HasValue && !End.Value.IsExpired() && (End.Value > DateTime.Today.AddDays(n.Value))) return true;
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
            if (End != null && End.HasValue && End.Value.IsToday())
            {
                return true;
            }

            return false;
        }
        public bool IsInDay(DateTime date)
        {
            if (End != null && End.HasValue && (End.Value.Day == date.Day)) return true;
            return false;
        }
        public bool IsCompleted()
        {
            if (Status == IssueStatus.Done) return true;
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
            if (Content.IsNullOrEmptyOrWhiteSpace()) return;
            DateTime[] _times = Content.GetTimes();
            if (_times != null && _times.Count() > 0)
            {
                foreach (var _time in _times)
                {
                    Times.Add(_time);
                }
            }
            if (!Content.Contains("/")) return;
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
        }

        public bool HasKeywords(string keyWord)
        {
            if (keyWord == null || keyWord.IsNullOrEmptyOrWhiteSpace()) return false;
            keyWord = keyWord.ToLower();
            if (Content.IsNullOrEmptyOrWhiteSpace())
            {
                return false;
            }
            if (Content.ToLower().Contains(keyWord))
            {
                return true;
            }
            return false;
        }

        public bool IsValid()
        {
            if (!Start.HasValue) return false;
            if (!End.HasValue) return false;
            if (Title.IsNullOrEmptyOrWhiteSpace() && Content.IsNullOrEmptyOrWhiteSpace()) return false;
            return true;
        }

        public bool IsInMonth(int month)
        {
            Update();
            if (End.Value.Month == month) return true;
            return false;
        }

        public void PrepareToCreate()
        {
            if (!IsValid()) return;
            if(Title.IsNullOrEmptyOrWhiteSpace())
            {
                if (!Content.IsNullOrEmptyOrWhiteSpace())
                {
                    HtmlToText convert = new HtmlToText();
                    string _text = convert.Convert(Content);
                    Title = _text.GetSentences().FirstOrDefault();
                }
            }
            
            if (Price <= 0) Price = IssueEstimation * Salary;

            GetSchedule();
            if(IsNoTime())
            {
                MoveNow();
            }
            else
            {
                AutoSetTimes();
            }
        }

        public ScheduleType GetSchedule()
        {
            if (Title.ToLower().Contains("[daily]"))
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
            
            return Repeat;
        }

        public bool HasTag()
        {
            if (Content.IsNullOrEmptyOrWhiteSpace()) return false;
            else
            {
                string[] _hasTags = Content.GetHasTags();
                if (_hasTags != null && _hasTags.Count() > 0)
                {
                    foreach (string _hasTag in _hasTags)
                    {
                        HasTags.Add(_hasTag);
                    }
                }
                return true;
            }
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

        public string[] GetLinks()
        {
            List<string> _links = new List<string>();
            _links = Content.GetUrls().Distinct().ToList();
            return _links.ToArray();
        }
        public void MoveToday()
        {
            Start = DateTime.Today.StartWorkingTime();
            End = DateTime.Today.StartWorkingTime().AddMinutes(IssueEstimation);
        }

        public void MoveNow()
        {
            this.Start = DateTime.Now;
            this.End = DateTime.Now.AddMinutes(IssueEstimation);
        }

        public void SetTomorrow()
        {
            this.Start = DateTime.Today.AddDays(1).StartWorkingTime();
            this.End = DateTime.Today.AddDays(1).StartWorkingTime().AddMinutes(IssueEstimation);
        }

        public string GetMessage()
        {
            Message = string.Empty;

            if (!Start.HasValue)
            {
                Message += "\n Must setup [Start]";
            }

            if (!End.HasValue)
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
        public bool Contains(string keyword)
        {
            if (!Title.IsNullOrEmptyOrWhiteSpace() && Title.ToLower().Contains(keyword.ToLower())) return true;
            if (!Content.IsNullOrEmptyOrWhiteSpace() &&  Content.ToLower().Contains(keyword.ToLower())) return true;
            return false;
        }
        public void AddTimeToDo(int minutes)
        {
            IssueEstimation = IssueEstimation + minutes;
        }

        public string[] GetEmails()
        {
            if (Title.HasEmail()) Emails = Title.GetEmails().ToList();
            if (Content.HasEmail()) Emails.AddRange(Content.GetEmails().ToList());
            Emails.Add(CreatedBy);
            return Emails.Distinct().ToArray();
        }
        
        public string[] GetCommands()
        {
            List<string> commands = new List<string>();
            string[] _patterns = new string[] { "{,}", "[,]", "$$,$$" };
            foreach (string _pattern in _patterns)
            {
                string open = _pattern.Split(new string[] { "," }, StringSplitOptions.None)[0];
                string close = _pattern.Split(new string[] { "," }, StringSplitOptions.None)[1];
                var _commands = Content.ExtractAllBetween(open, close);
                if (_commands != null && _commands.Count() > 0)
                {
                    commands.AddRange(_commands.ToList());
                }
            }

            return commands.ToArray();
        }

        #region Notification
        public void Notify(EmailService emailService)
        {
            var _emails = GetEmails().Distinct();
            if (_emails != null && _emails.Count() > 0)
            {
                foreach (string _email in _emails)
                {
                    emailService.SendAsync(this, _email);
                }
            }
        }
        #endregion

        public bool IsRunning()
        {
            if(Start.HasValue && End.HasValue && Start.Value.IsInRange(DateTime.Now, End.Value))
            {
                return true;
            }
            return false;
        }
    }
}