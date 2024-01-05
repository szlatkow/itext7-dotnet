/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2024 Apryse Group NV
    Authors: Apryse Software.

    This program is offered under a commercial and under the AGPL license.
    For commercial licensing, contact us at https://itextpdf.com/sales.  For AGPL licensing, see below.

    AGPL licensing:
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */
using System;
using System.IO;
using System.Net;

namespace iText.IO.Util {
    /// <summary>
    /// This file is a helper class for internal usage only.
    /// Be aware that its API and functionality may be changed in future.
    /// </summary>
    public static class UrlUtil {
        /// <summary>This method makes a valid URL from a given filename.</summary>
        /// <param name="filename">a given filename</param>
        /// <returns>a valid URL</returns>
        public static Uri ToURL(String filename) {
            try {
                return new Uri(filename);
            } catch {
                return new Uri(Path.GetFullPath(filename));
            }
        }

        public static Stream OpenStream(Uri url) {
            Stream isp;
            if (url.IsFile) {
                // Use url.LocalPath because it's needed for handling UNC pathes (like used in local
                // networks, e.g. \\computer\file.ext). It's safe to use #LocalPath because we 
                // check #IsFile beforehand. On the other hand, the url.AbsolutePath provides escaped string and also breaks
                // UNC path.
                isp = new FileStream(url.LocalPath, FileMode.Open, FileAccess.Read);     
            } else {
                WebRequest req = WebRequest.Create(url);
                req.Credentials = CredentialCache.DefaultCredentials;
                using (WebResponse res = req.GetResponse())
                using (Stream rs = res.GetResponseStream()) {
                    // We don't want to leave the response stream in an open state as it
                    // may lead to running out of server connections what will block processing
                    // of new requests. Therefore copying the state of the stream to a MemoryStream 
                    // which doesn't deal with connections.
                    isp = new MemoryStream();
                    byte[] buffer = new byte[4096];
                    int read;
                    while ((read = rs.Read(buffer, 0, buffer.Length)) > 0) {
                        isp.Write(buffer, 0, read);
                    }
                    isp.Position = 0;
                }
            }

            return isp;
        }

        /// <summary>
        /// This method makes a normalized URI from a given filename. 
        /// </summary>
        /// <param name="filename">a given filename</param>
        /// <returns>a valid Uri</returns>
        public static Uri ToNormalizedURI(String filename) {
            return ToNormalizedURI(new FileInfo(filename));
        }

        /// <summary>
        /// This method makes a normalized URI from a given file. 
        /// </summary>
        /// <param name="file">a given file</param>
        /// <returns>a valid Uri</returns>
        public static Uri ToNormalizedURI(FileInfo file) {
            return new Uri(file.FullName);
        }
        
        /// <summary>
        /// This method gets uri string from a file.
        /// </summary>
        /// <param name="filename">a given filename</param>
        /// <returns>a uri string</returns>
        public static String GetFileUriString(String filename)
        {
            return new FileInfo(filename).FullName;
        }
        
        /// <summary>
        /// This method gets normalized uri string from a file.
        /// </summary>
        /// <param name="filename">a given filename</param>
        /// <returns>a normalized uri string</returns>
        public static String GetNormalizedFileUriString(String filename)
        {
            return "file://" + UrlUtil.ToNormalizedURI(filename).AbsolutePath;
        }

        /// <summary>
        /// Gets the input stream of connection related to last redirected url. You should manually close input stream after
        /// calling this method to not hold any open resources.
        /// </summary>
        /// <param name="url">an initial URL.</param>
        /// 
        /// <returns>an input stream of connection related to the last redirected url.</returns>
        public static Stream GetInputStreamOfFinalConnection(Uri url) {
            return UrlUtil.OpenStream(url);
        }
    }
}
