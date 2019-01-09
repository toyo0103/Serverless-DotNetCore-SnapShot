using System;
using System.Collections.Generic;
using System.Text;

namespace SnapshotDemo.BE
{
    /// <summary>
    /// SQS Event Body
    /// </summary>
    public class SqsEventBodyEntity
    {
        /// <summary>
        /// Gets or sets the snapshot URL.
        /// </summary>
        /// <value>
        /// The snapshot URL.
        /// </value>
        public string SnapshotUrl { get; set; }
    }
}
