// 
// Copyright (c) 2004-2018 Jaroslaw Kowalski <jaak@jkowalski.net>, Kim Christensen, Julian Verdurmen
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

namespace NLog.LayoutRenderers
{
    using System;
    using System.ComponentModel;
    using System.Text;
    using NLog.Config;
	using NLog.Internal;

    /// <summary>
    /// Globally-unique identifier (GUID).
    /// </summary>
    [LayoutRenderer("guid")]
    [ThreadSafe]
    public class GuidLayoutRenderer : LayoutRenderer, IRawValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuidLayoutRenderer" /> class.
        /// </summary>
        public GuidLayoutRenderer()
        {
            Format = "N";
        }

        /// <summary>
        /// Gets or sets the GUID format as accepted by Guid.ToString() method.
        /// </summary>
        /// <docgen category='Rendering Options' order='10' />
        [DefaultValue("N")]
        public string Format { get; set; }

        /// <summary>
        /// Generate the Guid from the NLog LogEvent (Will be the same for all targets)
        /// </summary>
        /// <docgen category='Rendering Options' order='100' />
        [DefaultValue(false)]
        public bool GeneratedFromLogEvent { get; set; }

        /// <summary>
        /// Get raw value
        /// </summary>
        object IRawValue.GetRawValue(LogEventInfo logEvent) => GetValue(logEvent);

        /// <summary>
        /// Renders a newly generated GUID string and appends it to the specified <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to append the rendered data to.</param>
        /// <param name="logEvent">Logging event.</param>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            Guid guid;
            guid = GetValue(logEvent);
            builder.Append(guid.ToString(Format));
        }

        private Guid GetValue(LogEventInfo logEvent)
        {
            Guid guid;
            if (GeneratedFromLogEvent)
            {
                int hashCode = logEvent.GetHashCode();
                short b = (short)((hashCode >> 16) & 0XFFFF);
                short c = (short)(hashCode & 0XFFFF);
                long zeroDateTicks = LogEventInfo.ZeroDate.Ticks;
                byte d = (byte)((zeroDateTicks >> 56) & 0xFF);
                byte e = (byte)((zeroDateTicks >> 48) & 0xFF);
                byte f = (byte)((zeroDateTicks >> 40) & 0xFF);
                byte g = (byte)((zeroDateTicks >> 32) & 0xFF);
                byte h = (byte)((zeroDateTicks >> 24) & 0xFF);
                byte i = (byte)((zeroDateTicks >> 16) & 0xFF);
                byte j = (byte)((zeroDateTicks >> 8) & 0xFF);
                byte k = (byte)(zeroDateTicks & 0XFF);
                guid = new Guid(logEvent.SequenceID, b, c, d, e, f, g, h, i, j, k);

            }
            else
            {
                guid = Guid.NewGuid();
            }

            return guid;
        }
    }
}