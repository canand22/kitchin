using System;
using System.Threading;
using KitchIn.Core.Services.Yummly;

namespace KitchIn.Core.Services.Jobs
{
    public class YummlyMetaUpdater : IRunable
    {
        private readonly Timer timer;
        private IYummly yummlyManager;

        public YummlyMetaUpdater(IYummly yummplyManager)
        {
            this.yummlyManager = yummplyManager;
            this.timer = new Timer(this.MetaUpdater, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void Run()
        {
            var nextStartTs = (new TimeSpan(DateTime.Now.Hour, 0, 0) - DateTime.Now.TimeOfDay).TotalSeconds < 0
                            ? new TimeSpan(DateTime.Now.Hour + 1, 0, 0) - DateTime.Now.TimeOfDay
                            : new TimeSpan(DateTime.Now.Hour, 0, 0) - DateTime.Now.TimeOfDay;

            this.timer.Change(nextStartTs, new TimeSpan(1, 0, 0));

        }

        public void MetaUpdater(object state)
        {
            yummlyManager.UpdateMetadata();
        }
    }
}
