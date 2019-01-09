using System.Diagnostics;
using System.IO;
using System.Linq;
using Amazon.Lambda.SQSEvents;
using Newtonsoft.Json;
using SnapshotDemo.BE;
using SnapshotDemo.Consts;

namespace SnapshotDemo
{
    public class SqsHandler
    {
        /// <summary>
        /// 執行快照
        /// </summary>
        /// <param name="sqsEvent">The SQS event.</param>
        /// <returns>執行結果</returns>
        public string Snapshot(SQSEvent sqsEvent)
        {
            this.Initial();
            var snapshotService = new SalePageSnapshot();

            ////TO DO
            var record = sqsEvent.Records.First();
            var parameter = JsonConvert.DeserializeObject<SqsEventBodyEntity>(record.Body);
            snapshotService.Do(parameter.SnapshotUrl);


            return "拍照完成";
        }

        /// <summary>
        /// Isinitialeds this instance.
        /// </summary>
        private void Initial()
        {
            if (!File.Exists(PathConst.ChromeDriver))
            {
                File.Copy("/var/task/chromedriver", PathConst.ChromeDriver, true);
                this.Exec($"chmod +x {PathConst.ChromeDriver}");
            }

            if (!File.Exists(PathConst.Chromium))
            {
                File.Copy("/var/task/headless-chromium", PathConst.Chromium, true);
                this.Exec($"chmod +x {PathConst.Chromium}");
            }
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="cmd">The command.</param>
        private void Exec(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\""
                }
            };

            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
        }
    }
}

