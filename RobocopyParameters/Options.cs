using System;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using Plossum.CommandLine;
using Fonlow.CommandLine;

namespace Fonlow.CommandLineGui.Robocopy
{
    sealed class OptionGroups
    {
        OptionGroups()
        {

        }
        public const string TOP_CATEGORY = " Parameters";
        public const string COPY_OPTIONS = "Copy Options";
        public const string LOGGING_OPTIONS = "Logging Options";
        public const string RETRY_OPTIONS = "Retry Options";
        public const string FILE_SELECTION_OPTIONS = "File Selection Options";
    }


    /// <summary>
    /// Contains arguments of a robocopy command, including default value, description and editor for each argument.
    /// </summary>
    [CommandLineOptionGroup(OptionGroups.COPY_OPTIONS)]
    [CommandLineOptionGroup(OptionGroups.LOGGING_OPTIONS)]
    [CommandLineOptionGroup(OptionGroups.RETRY_OPTIONS)]
    [CommandLineOptionGroup(OptionGroups.FILE_SELECTION_OPTIONS)]
    [CommandLineManager(ApplicationName = "Robocopy", Description = "Robocopy options", RequireExplicitAssignment = true)]
    public class RobocopyOptions : IRefineOptions
    {
        [FixedParameter(Category = OptionGroups.TOP_CATEGORY, Description = @"Source Directory (drive:\path or \\server\share\path).",
            DisplayName = "Source", Order = 1, DefaultValue = "SourceDir")]
        public string Source { get; set; }


        [FixedParameter(Category = OptionGroups.TOP_CATEGORY, Description = @"Destination Dir  (drive:\path or \\server\share\path).",
           DisplayName = "Destination", Order = 2, DefaultValue = "DestDir")]
        public string Destination { get; set; }

        [FixedParameter(Category = OptionGroups.TOP_CATEGORY, Description = "File(s) to copy  (names/wildcards: default is \"*.*\").",
         DisplayName = "Files", Order = 3, DefaultValue = "FilesSeparatedBySpace")]
        public string[] Files { get; set; }

        [CommandLineOption(Name = "?",
            Description = "Usage info and help.")]
        public bool Help
        { get; set; }


        #region COPY OPTIONS

        [CommandLineOption(Name = "S", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Copies subdirectories (excluding empty ones).")]
        public bool SlashS
        {
            get;
            set;
        }


        [CommandLineOption(Name = "E", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Copies all subdirectories (including empty ones).")]
        public bool SlashE
        {
            get;
            set;
        }



        [CommandLineOption(Name = "LEV", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Copies only the top n levels of the source directory tree.")]
        public int SlashLev { get; set; }


        [CommandLineOption(Name = "Z", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Copies files in restartable mode (that is, restarts the copy process from the point of failure).")]
        public bool SlashZ { get; set; }


        [CommandLineOption(Name = "B", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Copies files in Backup mode (Backup copies are not restartable, but can copy some files that restartable mode cannot).")]
        public bool SlashB { get; set; }


        [CommandLineOption(Name = "ZB", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Tries to copy files in restartable mode, but if that fails with an “Access Denied” error, switches automatically to Backup mode.")]
        public bool SlashZB { get; set; }



        const CopyFlags fullCopy = CopyFlags.A | CopyFlags.D | CopyFlags.O | CopyFlags.S | CopyFlags.T | CopyFlags.U;
        const CopyFlags secCopy = CopyFlags.D | CopyFlags.A | CopyFlags.T | CopyFlags.S;



        [Editor(typeof(CopyFlagsEditor), typeof(UITypeEditor))]
        [CommandLineOption(Name = "COPY", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Copies the file information specified by copyflags, which can be any combination of the following :" + "\n\r" +
"D – file Data. S – file Security (NTFS ACLs)." + "\n\r" +
"A – file Attributes. O – file Ownership information." + "\n\r" +
"T – file Timestamps. U – file aUditing infomation." + "\n\r" +
"Source and destination volumes must both be NTFS to copy Security, Ownership or Auditing information.")]
        public CopyFlags SlashCopy
        {
            get;
            set;

        }



        [CommandLineOption(Name = "COPYALL", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Copies Everything. Equivalent to /COPY:DATSOU.")]
        public bool SlashCopyAll
        {
            get;
            set;
        }


        [CommandLineOption(Name = "EFSRAW", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Copies all encrypted files in EFS RAW mode.")]
        public bool SlashEfsRaw { get; set; }


        [CommandLineOption(Name = "DCOPY:T", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Copies directory time stamps.")]
        public bool SlashDCopyT { get; set; }


        [CommandLineOption(Name = "SEC", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Copies NTFS security information. (Source and destination volumes must both be NTFS). Equivalent to /COPY:DATS.")]
        public bool SlashSec
        {
            get;
            set;
        }


        [CommandLineOption(Name = "NOCOPY", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Copies Nothing. Can be useful with /PURGE.")]
        public bool SlashNoCopy { get; set; }


        [CommandLineOption(Name = "SECFIX", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Fixes file security on all files, even skipped ones.")]
        public bool SlashSecFix { get; set; }


        [CommandLineOption(Name = "TIMFIX", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Fixes file times on all files, even skipped ones.")]
        public bool SlashTimFix { get; set; }


        [CommandLineOption(Name = "PURGE", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Deletes destination files and directories that no longer exist in the source.")]
        public bool SlashPurge
        {
            get;
            set;

        }


        [CommandLineOption(Name = "MIR", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Mirrors a directory tree (equivalent to running both /E and /PURGE).")]
        public bool SlashMir { get; set; }



        [CommandLineOption(Name = "MOV", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Moves files (that is, deletes source files after copying).")]
        public bool SlashMov
        {
            get;
            set;
        }


        [CommandLineOption(Name = "MOVE", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Moves files and directories (that is, deletes source files and directories after copying).")]
        public bool SlashMovE
        {
            get;
            set;

        }



        [Editor(typeof(RashFlagsEditor), typeof(UITypeEditor))]
        [CommandLineOption(Name = "A+", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Sets the specified attributes in copied files.\n\rThe following attributes can be set:\n\r" +
"R – Read only S – System N – Not content indexed\n\rA – Archive H – Hidden T – Temporary")]
        public Rashcneto SlashAPlus
        {
            get;
            set;
        }

        const Rashcneto fullRashcneto = Rashcneto.A | Rashcneto.C | Rashcneto.E | Rashcneto.H | Rashcneto.N
            | Rashcneto.O | Rashcneto.R | Rashcneto.S | Rashcneto.T;




        [Editor(typeof(RashFlagsEditor), typeof(UITypeEditor))]
        [CommandLineOption(Name = "A-", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Turns off the specified attributes in copied files.\n\rThe following attributes can be turned off:\n\r" +
"R – Read only S – System N – Not content indexed\n\rA – Archive H – Hidden T – Temporary")]
        public Rashcneto SlashAMinus
        {
            get;
            set;
        }


        [CommandLineOption(Name = "CREATE", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Creates a directory tree structure containing zero-length files only (that is, no file data is copied).")]
        public bool SlashCreate { get; set; }

        [CommandLineOption(Name = "FAT", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Creates destination files using only 8.3 FAT file names.")]
        public bool SlashFat { get; set; }

        [CommandLineOption(Name = "256", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Turns off support for very long paths (longer than 256 characters).")]
        public bool Slash256 { get; set; }



        [CommandLineOption(Name = "MON", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Monitors the source directory for changes, and runs again when n changes have been detected, and the minimum time interval specified by /MOT has elapsed.")]
        public int SlashMon { get; set; }

        [CommandLineOption(Name = "MOT", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Monitors the source directory for changes, and runs again when a further n minutes have elapsed, and the minimum number of changes specified by /MON have been detected.")]
        public int SlashMot { get; set; }


        [CommandLineOption(Name = "RH", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Defines the time slot during which starting new copies is allowed. Useful for restricting copies to certain times of the day. Both values must be 24-hour times in the range 0000 to 2359.")]
        public string SlashRh { get; set; }



        [CommandLineOption(Name = "PF", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Makes more frequent checks to see if starting new copies is allowed (per file rather than per pass). Useful in stopping copy activity more promptly at the end of the run hours time slot.")]
        public bool SlashPf { get; set; }


        [CommandLineOption(Name = "IPG", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Inserts a delay of n milliseconds after each 64k chunk of file data is copied. Useful for freeing up bandwidth on slow lines.")]
        public int SlashIpg { get; set; }



        [CommandLineOption(Name = "SL", GroupId = OptionGroups.COPY_OPTIONS,
            Description = "Copy symbolic links versus the target.")]
        public bool SlashSl { get; set; }

        #endregion


        #region File Selection Options


        [CommandLineOption(Name = "A", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Copies only files with the archive attribute set.")]
        public bool SlashA { get; set; }


        [CommandLineOption(Name = "M", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Copies only files with the archive attribute set and then resets (turns off) the archive attribute in the source files.")]
        public bool SlashM { get; set; }


        [Editor(typeof(RashFlagsEditor), typeof(UITypeEditor))]
        [CommandLineOption(Name = "IA", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Includes files with the specified attributes.\n\rThe following file attributes can be acted upon:\n\r" +
"R – Read only A – Archive S – System\n\rH – Hidden C – Compressed N – Not content indexed\n\rE – Encrypted T – Temporary O - Offline")]
        public Rashcneto SlashIa { get; set; }

        [Editor(typeof(RashFlagsEditor), typeof(UITypeEditor))]
        [CommandLineOption(Name = "XA", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes files with the specified attributes.\n\rThe following file attributes can be acted upon:\n\r" +
"R – Read only A – Archive S – System\n\rH – Hidden C – Compressed N – Not content indexed\n\rE – Encrypted T – Temporary O - Offline")]
        public Rashcneto SlashXa { get; set; }



        [CommandLineOption(Name = "XF", GroupId = OptionGroups.FILE_SELECTION_OPTIONS, RequireExplicitAssignment = false,
            Description = "Excludes files with the specified names, paths, or wildcard characters.")]
        public string[] SlashXf
        {
            get;
            set;
        }

        [CommandLineOption(Name = "XD", GroupId = OptionGroups.FILE_SELECTION_OPTIONS, RequireExplicitAssignment = false,
            Description = "Excludes directories with the specified names, paths, or wildcard characters.")]
        public string SlashXd
        {
            get;
            set;
        }


        [CommandLineOption(Name = "XC", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes files tagged as “Changed”.")]
        public bool SlashXc { get; set; }

        [CommandLineOption(Name = "XN", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes files tagged as “Newer”.")]
        public bool SlashXn { get; set; }


        [CommandLineOption(Name = "XO", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes files tagged as “Older”..")]
        public bool SlashXo { get; set; }


        [CommandLineOption(Name = "XX", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes files and directories tagged as “Extra”.")]
        public bool SlashXx { get; set; }


        [CommandLineOption(Name = "XL", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes files and directories tagged as “Lonely”.")]
        public bool SlashXl { get; set; }

        [CommandLineOption(Name = "IS", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Includes files tagged as “Same”.")]
        public bool SlashIs { get; set; }

        [CommandLineOption(Name = "IT", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Includes files tagged as “Tweaked”.")]
        public bool SlashIt { get; set; }


        [CommandLineOption(Name = "MAX", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes files larger than n bytes.")]
        public int SlashMax { get; set; }


        [CommandLineOption(Name = "MIN", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes files smaller than n bytes.")]
        public int SlashMin { get; set; }

        [CommandLineOption(Name = "MAXAGE", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes files with a Last Modified Date older than n days or specified date. If n is less than 1900, then n is expressed in days. Otherwise, n is a date expressed as YYYYMMDD.")]
        public string SlashMaxAge { get; set; }


        [CommandLineOption(Name = "MINAGE", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes files with a Last Modified Date newer than n days or specified date. If n is less than 1900, then n is expressed in days. Otherwise, n is a date expressed as YYYYMMDD.")]
        public string SlashMinAge { get; set; }

        [CommandLineOption(Name = "MAXLAD", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes files with a Last Access Date older than n days or specified date. If n is less than 1900, then n is expressed in days. Otherwise, n is a date expressed as YYYYMMDD.")]
        public string SlashMaxLad { get; set; }


        [CommandLineOption(Name = "MINLAD", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes files with a Last Access Date newer than n days or specified date. If n is less than 1900, then n is expressed in days. Otherwise, n is a date expressed as YYYYMMDD.")]
        public string SlashMinLad { get; set; }


        [CommandLineOption(Name = "XJ", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes Junction points.")]
        public bool SlashXj { get; set; }


        [CommandLineOption(Name = "FFT", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Assume FAT File Times (2-second granularity). Useful for copying to third-party systems that declare a volume to be NTFS but only implement file times with a 2-second granularity.")]
        public bool SlashFft { get; set; }


        [CommandLineOption(Name = "DST", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Compensates for one-hour DST time differences.")]
        public bool SlashDst { get; set; }


        [CommandLineOption(Name = "XJD", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes junction points for directories.")]
        public bool SlashXjd { get; set; }

        [CommandLineOption(Name = "XJF", GroupId = OptionGroups.FILE_SELECTION_OPTIONS,
            Description = "Excludes junction points for files.")]
        public bool SlashXjf { get; set; }

        #endregion

        #region Retry Options


        [CommandLineOption(Name = "R", GroupId = OptionGroups.RETRY_OPTIONS,
            Description = "Specifies the number of retries on failed copies. (The default is 1 million.)")]
        public int SlashR { get; set; }


        [CommandLineOption(Name = "W", GroupId = OptionGroups.RETRY_OPTIONS,
           Description = "Specifies the wait time between retries. (The default is 30 seconds.)")]
        public int SlashW { get; set; }


        [CommandLineOption(Name = "REG", GroupId = OptionGroups.RETRY_OPTIONS,
         Description = "Saves /R:n and /W:n in the registry as default settings.")]
        public bool SlashReg { get; set; }


        [CommandLineOption(Name = "TBD", GroupId = OptionGroups.RETRY_OPTIONS,
         Description = "Waits for share names to be defined on a “Network Name Not Found” error.")]
        public bool SlashTbd { get; set; }
        #endregion


        #region Logging Options


        [CommandLineOption(Name = "L", GroupId = OptionGroups.LOGGING_OPTIONS,
          Description = "Lists files without copying, deleting, or applying a time stamp to any files.")]
        public bool SlashL { get; set; }

        [CommandLineOption(Name = "X", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Reports all files tagged as “Extra”(including files not selected).")]
        public bool SlashX { get; set; }

        [CommandLineOption(Name = "V", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Produces verbose output (including skipped files)..")]
        public bool SlashV { get; set; }

        [CommandLineOption(Name = "TS", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Displays source file timestamps in the output log.")]
        public bool SlashTs { get; set; }

        [CommandLineOption(Name = "FP", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Displays full pathnames of files in the output log.")]
        public bool SlashFp { get; set; }

        [CommandLineOption(Name = "BYTES", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Prints sizes, as bytes.")]
        public bool SlashBytes { get; set; }

        [CommandLineOption(Name = "NS", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Suppresses output of file and directory sizes.")]
        public bool SlashNs { get; set; }

        [CommandLineOption(Name = "NC", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Suppresses output of Robocopy file classes.")]
        public bool SlashNc { get; set; }

        [CommandLineOption(Name = "NFL", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Turns off logging of file names. File names are still shown, however, if file copy errors occur.")]
        public bool SlashNfl { get; set; }

        [CommandLineOption(Name = "NDL", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Turns off logging of directory names. Full file pathnames (as opposed to simple file names) will be shown if /NDL is used.")]
        public bool SlashNdl { get; set; }

        [CommandLineOption(Name = "NP", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Turns off copy progress indicator (% copied).")]
        public bool SlashNp { get; set; }

        [CommandLineOption(Name = "ETA", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Shows estimated time of completion for copied files.")]
        public bool SlashEta { get; set; }

        [CommandLineOption(Name = "LOG", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Redirects output to the specified file, overwriting the file if it already exists.")]
        public string SlashLog { get; set; }


        [CommandLineOption(Name = "LOGPlus", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Redirects output to the specified file, appending it to the file if it already exists.")]
        public string SlashLogPlus { get; set; }


        [CommandLineOption(Name = "UNLOG", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Output status to LOG file as UNICODE (overwrite existinglog).")]
        public string SlashUniLog { get; set; }

        [CommandLineOption(Name = "UNLOG+", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Output status to LOG file as UNICODE (append existinglog).")]
        public string SlashUniLogPlus { get; set; }


        [CommandLineOption(Name = "TEE", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Displays output in the console window, in addition to directing it to the log file specified by /LOG or /LOG+.")]
        public bool SlashTee { get; set; }

        [CommandLineOption(Name = "NJH", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Turns of logging of the job header.")]
        public bool SlashNjh { get; set; }

        [CommandLineOption(Name = "NJS", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Turns off logging of the job summary.")]
        public bool SlashNjs { get; set; }

        [CommandLineOption(Name = "UNICODE", GroupId = OptionGroups.LOGGING_OPTIONS,
            Description = "Output status as UNICODE.")]
        public bool SlashUnicode { get; set; }

        #endregion



        public void Refine()
        {
            if (SlashS)
            {
                SlashE = false;
            }


            if (SlashE)
            {
                SlashS = false;

                if (SlashPurge)
                {
                    SlashMir = true;
                }
            }

            if (SlashCopyAll)
            {

                SlashCopy = CopyFlags.None;
                System.Diagnostics.Trace.TraceInformation("When /COPYALL is set, /COPY is clear.");

                SlashSec = false;
                System.Diagnostics.Trace.TraceInformation("When /COPYALL is set, /SEC is clear.");
            }


            if (SlashCopy == fullCopy)
            {
                SlashCopyAll = true;
                SlashCopy = CopyFlags.None;
                System.Diagnostics.Trace.TraceInformation("/COPY:DATSOU is converted into /COPYALL.");
            }

            if ((SlashCopy & secCopy) == secCopy)
            {

                SlashSec = false;
                System.Diagnostics.Trace.TraceInformation("As /COPY:DATS is set, /SEC is clear.");
            }

            if (SlashSec)
            {
                SlashCopy |= secCopy;
                SlashSec = false;
                System.Diagnostics.Trace.TraceInformation("/SEC is converted into /COPY:DATS");
            }

            if (SlashE && SlashPurge)
            {
                SlashMir = true;
                SlashPurge = false;
            }

            if (SlashMov)
            {
                SlashPurge = SlashMir = SlashMovE = false;
            }

            if (SlashMovE)
            {
                SlashPurge = SlashMir = SlashMov = false;
            }


        }


        public void Validate()
        {
            StringBuilder builder = new StringBuilder();
            var r = SlashAPlus & SlashAMinus;
            if (r != Rashcneto.None)
            {
                builder.AppendLine(String.Format("Option A+ and Option A- have some conflicits, sharing common flags: {0}", r));
            }

            var r2 = SlashIa & SlashXa;
            if (r2 != Rashcneto.None)
            {
                builder.AppendLine(String.Format("Option Ia and Option Xa have some conflicits, sharing common flags: {0}", r));
            }

            var s = builder.ToString();
            if (!String.IsNullOrEmpty(s))
            {
                throw new InvalidParametersException(s);
            }
        }
    }




}
