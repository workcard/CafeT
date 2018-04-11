using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Objects.Enums
{
    public enum IssueStatus
    {
        New = 0,
        Doing = 1,
        Finished = 2,
        Done = 3,
        Testing = 4,
        ReOpen = 5
    }

    public enum ScheduleType
    {
        None = 0,
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        Quaterly = 4,
        Yearly = 5
    }

    public enum MoveTimeType
    {
        None = 0,
        Yesterday = 1,
        Today = 2,
        Tommorow = 3,
        NextWeek = 4,
        NextMonth = 5
    }

    public enum TimeTodoType
    {
        OneMinutes = 0,
        FiveMinutes = 1,
        TenMinutes = 2,
        FifthteenMinutes = 3,
        ThirtyMinutes = 4,
        SixtyHour = 5,
        OneDay = 6,
        HaftDay = 7
    }
}
