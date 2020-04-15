using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Wjire.Common;


namespace Wjire.ASP.NET.Core3._1.Demo
{

    /// <summary>
    /// Jwt黑名单清理服务
    /// </summary>
    public class CleaningJwtBlackCollectionService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private static readonly int IntervalHours = 1;

        public CleaningJwtBlackCollectionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <summary>
        /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running operations.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                RemoveExpiredToken();
                await Task.Delay(TimeSpan.FromHours(IntervalHours), stoppingToken);
            }
        }

        /// <summary>
        /// 移除黑名单中过期的token
        /// </summary>
        private void RemoveExpiredToken()
        {
            long min = DateTimeHelper.ConvertToTimeStamp(DateTime.Now.AddHours(-1 * Convert.ToDouble(_configuration[JwtConst.TokenExpiredHours])));
            RedisHelper.ZRemRangeByScore(JwtConst.JwtBlackCollection, 0, min);
        }
    }
}
