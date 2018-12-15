using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Configuration;
using System.Media;
using System.Speech;
using System.Speech.Synthesis;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;  
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.Monitoring;
using Microsoft.EnterpriseManagement.Monitoring.Security;


namespace OpsMgrAlertConsole
{
    enum ItemAge
    {
        New = 0,                // new added item
        Old = 1,                // existing item
        Deleted = 2,            // already deleted item (should be cleaned)
    };

    enum trace_flags
    {
        Trace2Con   = 0x0001,
        Trace2Event = 0x0002,
        Trace2File  = 0x0004,
        Trace2Scom  = 0x0010,
        Trace2Email = 0x0020,
        TraceOnFlag = 0x8000
    };

    /// <summary>
    /// Main console form
    /// </summary>
    public partial class frmMainWindow : Form
    {
        private Configuration appConfig;
        private NameValueCollection appSettings;
        private FileInfo configFile;
        private DateTime configTime;
        private Hashtable htAlerts;
        private Hashtable htComputers;
        private EventLog evtLog;

        private frmErrMsg fErrMessage = null;

        private ManagementGroup mgmtGroup = null;
        private ReadOnlyCollection<MonitoringAlert> monAlerts = null;
        private ReadOnlyCollection<PartialMonitoringObject> wndCompObjects = null;

        private uint uTraceFlags;

        private DateTime dtAlertsUpdated;           //last updated
        private DateTime dtMoveFG;              //last move foreground
        private DateTime dtMoveBG;              //last move background
        private int UpdInterval;

        private bool bCycleOpenWindows;             // Cycle console & open window (on top)
        private int iOpenWindowOnTopTimeout;

        private int EmailAging;                     // age < 5min --> gradient background
        private int iFontSizeAlert;
        private int iFontSizeComp;
        private int iFontColumnHeader;
        private int iFontSizeStatus;
        private string sMonAlertCriteria;
	    private bool bShowCompStateError;
	    private bool bShowCompStateWarning;
        private bool bShowCompInMaintenanceMode;
        private bool bShowCompStateUninitialized;

        private bool bUseCompiledAlertName;
        private bool bShowAcsAgentCount;
        private bool bShowLargeCompIcons;

        private string sFontFamily;
        private bool bSpeakAlert;
        private string sSpeakVoice;
        private bool bSplashAlert;
        private string sAlarmSound;
        private SoundPlayer player;

	    private bool bCameraPreviewEnable;
	    private string sCameraHostList;
	    private int iCameraRemotePort;
	    private string sCameraUserName;
	    private string sCameraPassword;
        private int iCameraShowTimeout;
        private string[] camHosts;
        private static int iCamNum = 0;
        private ULVIEWACTIVEXLib.ULVClass ulCam1;
        private string sCamSnapshot;

        private int iMaxACS = 0;

        private const int iColumnAlertName = 4;

        private SpeechSynthesizer speaker;

        //[DllImport("user32")]
        //public static extern int ShowCursor(int bShow);   

        private int iProcessedAlertCount = 0;
        private const int MaxSpeakAlerts = 4;
        private const int MaxSoundAlerts = 12;
        private const int MaxSplashAlerts = 30;
         

        #region Form Init

        /// <summary>
        /// Constuctor
        /// </summary>
        public frmMainWindow()
        {
            Trace.TraceInformation("frmMainWindow...start");
            
            InitializeComponent();

            // Get the configuration file.
            appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configFile = new FileInfo(appConfig.FilePath);
            configTime = configFile.LastWriteTime; 

            appSettings = ConfigurationManager.AppSettings;
  
            htAlerts = new Hashtable();
            htComputers = new Hashtable();

            evtLog = new EventLog();
            evtLog.Source = "OpsMgrAlertConsole";

            evtLog.WriteEntry("Start OpsMgrAlertConsole", EventLogEntryType.Information, 44401);

            Trace.TraceInformation("frmMainWindow...end");
            return;
        }

        /// <summary>
        /// Form Load
        /// </summary>
        private void frmMainWindow_Load(object sender, EventArgs e)
        {
            try
            {
                Trace.TraceInformation("frmMainWindow_Load...start");
                LoadConfigParams();

                /*
                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailAuxCallback);
                Image img1 = (Image)this.tssLblCrit.Image.GetThumbnailImage(img1.Width * 2, img1.Height * 2, myCallback, IntPtr.Zero);
                this.tssLblCrit.i 
                  */

                if (this.bShowLargeCompIcons)
                {
                    this.ssStatusBar.ImageScalingSize = new Size(32, 32);
                    this.lvComputers.SmallImageList = this.lvComputers.LargeImageList = this.imgIcons2;
                }
                else
                {
                    this.ssStatusBar.ImageScalingSize = new Size(16, 16);
                    this.lvComputers.SmallImageList = this.lvComputers.LargeImageList = this.imgIcons;
                }

                StringBuilder sbHdr = new StringBuilder();
                sbHdr.Append("OpsMgr Console v.");
                sbHdr.Append(Application.ProductVersion);
                sbHdr.Append("        : ");
                sbHdr.Append(configFile.LastWriteTime); 

                this.Text = sbHdr.ToString();

                if (bSpeakAlert)
                    PrepareVoice();

                SetAlertColums();

                timer2Wnd.Interval = iOpenWindowOnTopTimeout * 1000;
                timer3Cam.Interval = iCameraShowTimeout * 1000;

                this.txtCamHost.Visible = this.pictCam1.Visible = bCameraPreviewEnable;
                this.ulCam1 = null;
            }
            catch (Exception ex)
            {
                LogException("frmMainWindow_Load", ex);
            }

            Trace.TraceInformation("frmMainWindow_Load...end");
            return;
        }

        /// <summary>
        /// First form show
        /// </summary>
        private void frmMainWindow_Shown(object sender, EventArgs e)
        {
            Trace.TraceInformation("frmMainWindow_Shown...start");

            try
            {
                DateTime dt = DateTime.Now;
                dtAlertsUpdated = new DateTime(dt.Year - 1, 1, 1, 1, 1, 1);
                dtMoveFG = dtMoveBG = dtAlertsUpdated;

                timer1.Start();                         // start main timer for info async actions & updates

                if (bCycleOpenWindows) 
                    timer2Wnd.Start();                  // start windows cycle timer 

                if (bCameraPreviewEnable) 
                    timer3Cam.Start();                  // start cam preview windows

            }
            catch (Exception ex)
            {
                LogException("frmMainWindow_Shown", ex);
            }

            Trace.TraceInformation("frmMainWindow_Shown...end");
            return;
        }

        /// <summary>
        /// Set columns for alerts list
        /// </summary>
        private void SetAlertColums()
        {
            Trace.TraceInformation("SetAlertColums...start");

            string[] sColNames = new string[] {
            "Источник", "M", "Путь", "Приоритет", "Название", "Cоздано", "Возраст", "(R)", "Время"
            };

            HorizontalAlignment[] colAlign = new HorizontalAlignment[] {
                HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Left,
                HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Right,
                HorizontalAlignment.Right, HorizontalAlignment.Center, HorizontalAlignment.Right
            };

            try
            {
                string paramName;
                int iWidth;

                for (int i = 0; i < sColNames.Length; i++)
                {
                    paramName = "Column" + (i + 1).ToString();
                    iWidth = Convert.ToInt32(appSettings[paramName]);
                    Trace.Assert(iWidth >= 0 && iWidth <= 500, "ColumnWidth should be (0-500)");

                    lvAlerts.Columns.Add(sColNames[i], iWidth, colAlign[i]);
                }


                lvAlerts.ListViewItemSorter = new lvAlertComparer();
                lvComputers.ListViewItemSorter = new lvCompComparer();


                Font f1 = new Font(sFontFamily, iFontSizeAlert);
                lvAlerts.Font = f1;

                Font f2 = new Font(sFontFamily, iFontSizeComp);
                lvComputers.Font = f2;

                //Font f2 = new Font("Tahoma", 10);

                this.SetStyle(ControlStyles.DoubleBuffer |
                        ControlStyles.UserPaint |
                        ControlStyles.AllPaintingInWmPaint,
                        true);
                this.UpdateStyles();


                ResizeColumnWidth();

                tssAlert.Font = tssAll.Font = tssCrit.Font = tssWarn.Font
                    = tssOkey.Font = tssNull.Font = tssAcs.Font = tssTime.Font = f2;

                Font f3 = new Font(sFontFamily, iFontSizeStatus);
                tssLblAlert.Font = tssLblAll.Font = tssLblAcs.Font = tssLblNull.Font = f3;  

            }
            catch (Exception ex)
            {
                LogException("SetAlertColums", ex);
            }

            Trace.TraceInformation("SetAlertColums...end");
        }

        /// <summary>
        /// Resize column width to fill main form (full sum width)
        /// </summary>
        private void ResizeColumnWidth()
        {
            Trace.TraceInformation("ResizeColumnWidth...start");

            int sumWidth = (new VScrollBar()).Width;

            foreach (ColumnHeader colH in lvAlerts.Columns)
                sumWidth += colH.Width;

            double wRatio = (double)lvAlerts.ClientSize.Width / (double)sumWidth;

            if (wRatio != 1)
                foreach (ColumnHeader colH in lvAlerts.Columns)
                    colH.Width = (int)((double)colH.Width * wRatio);

            Trace.TraceInformation("ResizeColumnWidth...end");
        }


        /// <summary>
        /// Form Config params
        /// </summary>
        private void LoadConfigParams()
        {
            Trace.TraceInformation("LoadConfigParams...start");
            try
            {
                // Force a reload of the changed section.
                ConfigurationManager.RefreshSection("appSettings");
                configFile.Refresh();

                uTraceFlags = Convert.ToUInt32(appSettings["TraceFlags"],16);
                Trace.Assert(uTraceFlags <= 0x8FFF, "TraceFlags is incorrect");

                UpdInterval = Convert.ToInt32(appSettings["UpdateInterval"]);
                Trace.Assert(UpdInterval >= 30, "Minimal update inteval is 30 sec");

   	            bCycleOpenWindows = Convert.ToBoolean(appSettings["CycleOpenWindows"]);

                iOpenWindowOnTopTimeout = Convert.ToInt32(appSettings["OpenWindowOnTopTimeout"]);
                Trace.Assert(iOpenWindowOnTopTimeout >= 30, "Minimal OpenWindowOnTopTimeout is 30 sec");
                Trace.Assert(iOpenWindowOnTopTimeout < UpdInterval, "Maximum OpenWindowOnTopTimeout should be < UpdInterval");

                EmailAging = Convert.ToInt32(appSettings["EmailAging"]);
                Trace.Assert(UpdInterval >= 2, "Minimal EMail aging is 2 min");

                iFontSizeAlert = Convert.ToInt32(appSettings["FontSizeAlert"]);
                Trace.Assert(iFontSizeAlert >= 8 && iFontSizeAlert <= 40, "FontSizeAlert should be (8-40)");

                iFontSizeComp = Convert.ToInt32(appSettings["FontSizeComp"]);
                Trace.Assert(iFontSizeComp >= 8 && iFontSizeComp <= 40, "FontSizeComp should be (8-40)");

                iFontColumnHeader = Convert.ToInt32(appSettings["FontColumnHeader"]);
                Trace.Assert(iFontColumnHeader >= 8 && iFontColumnHeader <= 40, "FontColumnHeader should be (8-40)");

                iFontSizeStatus = Convert.ToInt32(appSettings["FontSizeStatus"]);
                Trace.Assert(iFontSizeStatus >= 8 && iFontSizeStatus <= 40, "FontSizeStatus should be (8-40)");

                sMonAlertCriteria = appSettings["MonitoringAlertCriteria"];

	            bShowCompStateError = Convert.ToBoolean(appSettings["ShowCompStateError"]);
	            bShowCompStateWarning = Convert.ToBoolean(appSettings["ShowCompStateWarning"]);
                bShowCompInMaintenanceMode = Convert.ToBoolean(appSettings["ShowCompInMaintenanceMode"]);
	            bShowCompStateUninitialized = Convert.ToBoolean(appSettings["ShowCompStateUninitialized"]);

                bUseCompiledAlertName = Convert.ToBoolean(appSettings["UseCompiledAlertName"]);
                bShowAcsAgentCount = Convert.ToBoolean(appSettings["ShowAcsAgentCount"]);

                bShowLargeCompIcons = Convert.ToBoolean(appSettings["ShowLargeCompIcons"]);
    
                sFontFamily = appSettings["FontFamily"];

                bSpeakAlert = Convert.ToBoolean(appSettings["SpeakAlert"]);
                sSpeakVoice = appSettings["SpeakVoice"];

                bSplashAlert = Convert.ToBoolean(appSettings["SplashAlert"]);
                sAlarmSound = appSettings["AlarmSound"];

                bCameraPreviewEnable = Convert.ToBoolean(appSettings["CameraPreviewEnable"]);

	            sCameraHostList = appSettings["CameraHostList"];
                camHosts = sCameraHostList.Split(' '); 

                iCameraRemotePort = Convert.ToInt32(appSettings["CameraRemotePort"]);
                sCameraUserName = appSettings["CameraUserName"];
                sCameraPassword = appSettings["CameraPassword"];
                iCameraShowTimeout = Convert.ToInt32(appSettings["CameraShowTimeout"]);
                Trace.Assert(iCameraShowTimeout >= 10, "CameraShowTimeout should be >= 10");

                try
                {
                    perfCntACS.MachineName = appSettings["ACShost"];
                    perfCntACS.CategoryName = "ACS Collector";
                    perfCntACS.CounterName = "Connected Clients";
                }
                catch (Exception ex1)
                {
                    LogException("LoadConfigParams - perfCntACS", ex1, false);
                }

                if (sAlarmSound != "system")
                {
                    player = new SoundPlayer(sAlarmSound);
                    player.Load();
                }

            }
            catch (Exception ex)
            {
                LogException("LoadConfigParams", ex);
            }

            Trace.TraceInformation("LoadConfigParams...end");
            return;
        }

        /// <summary>
        /// Prepare Voice for speaking
        /// </summary>
        void PrepareVoice()
        {
            Trace.TraceInformation("PrepareVoice...start");
            try
            {
                speaker = new SpeechSynthesizer();
                speaker.SetOutputToDefaultAudioDevice();
                try
                {
                    speaker.SelectVoice(sSpeakVoice);
                }
                catch
                {
                    speaker.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
                }
                //speaker.RemoveLexicon(new Uri("DB"));
                //speaker.RemoveLexicon(new Uri("RU"));

                //speaker.AddLexicon();
            }
            catch (Exception ex)
            {
                LogException("PrepareVoice", ex);
            }

            Trace.TraceInformation("PrepareVoice...end");
            return;
        }

        #endregion

        #region SCOM load data

        /// <summary>
        /// Load SCOM info (run async from timer)
        /// </summary>
        /// <param name="iParam">Some parameter</param>
        void LoadScomInfo(int iParam)
        {
            Trace.TraceInformation("LoadScomInfo({0})...start", iParam);

            try
            {
                if (mgmtGroup == null)                  // Connect to SCOM RMS
                    ConnectMgmtGroup();
                if (mgmtGroup == null)
                    return;
                                                        // Reconnect 
                if (mgmtGroup.IsConnected == false)
                    mgmtGroup.Reconnect();

                MonitoringClass wndCompClass;           // computer class 
                wndCompClass = mgmtGroup.GetMonitoringClass(SystemMonitoringClass.WindowsComputer);
                if (wndCompClass == null)
                    throw new Exception("Error GetMonitoringClass");

                this.wndCompObjects = null;
                this.monAlerts = null;
                                                        // load computer collection
                // evtLog.WriteEntry("GetPartialMonitoringObjects: SystemMonitoringClass.WindowsComputer",
                //                        EventLogEntryType.Information, 44403);
                this.wndCompObjects = mgmtGroup.GetPartialMonitoringObjects(wndCompClass);
                if (this.wndCompObjects == null)
                    throw new Exception("Error GetPartialMonitoringObjects");

                                                        // load alert collection
                //evtLog.WriteEntry("GetMonitoringAlerts: 'ResolutionState=0 AND Severity=2'",
                //    EventLogEntryType.Information, 44404);
                MonitoringAlertCriteria alertCriteria =
                    new MonitoringAlertCriteria(sMonAlertCriteria);
                this.monAlerts = mgmtGroup.GetMonitoringAlerts(alertCriteria);
                if (this.monAlerts == null)
                    throw new Exception("Error GetMonitoringAlerts");
            }
            catch (Exception ex)
            {
                LogException("LoadScomInfo", ex);
            }

            Trace.TraceInformation("LoadScomInfo({0})...end", iParam);
            return;
        }

        /// <summary>
        /// Connect to RMS
        /// </summary>
        private void ConnectMgmtGroup()
        {
            Trace.TraceInformation("ConnectMgmtGroup...start");

            string sRMS = appSettings["RMShost"];
            try
            {
                evtLog.WriteEntry("Connect to RMS: " + sRMS, EventLogEntryType.Information, 44402);
                mgmtGroup = new ManagementGroup(sRMS);
                if (mgmtGroup == null)
                    throw new Exception("Error connecting to RMS:" + sRMS);
            }
            catch (Exception ex)
            {
                LogException("ConnectMgmtGroup:" + sRMS, ex);
            }

            Trace.TraceInformation("ConnectMgmtGroup...end");
            return;
        }


        #endregion

        #region Form filling


        /// <summary>
        /// Update Alerts list
        /// </summary>
        protected void UpdateAlertList()
        {
            Trace.TraceInformation("UpdateAlertList...start");

            try
            {
                int nPrevAlerts = lvAlerts.Items.Count;             // =0 if 1st time filling
                tssAlert.Text = monAlerts.Count.ToString();

                //lvAlerts.Sorting = SortOrder.None;
                foreach (ListViewItem lviPrev in lvAlerts.Items)
                {
                    AlertItem alItem = (AlertItem)lviPrev.Tag;
                    if (alItem.itemAge==ItemAge.Deleted)
                    {
                        htAlerts.Remove(alItem.monAlert.Id);     // remove prev deleted items
                        lviPrev.Remove();
                    }
                    else
                        alItem.itemAge = ItemAge.Deleted;         // mark to delete (if not changed later)
                }

                foreach (MonitoringAlert alert in monAlerts)
                {
                    AlertItem alItem;
                    ListViewItem lviAlert;

                    if (htAlerts.ContainsKey(alert.Id))
                    {
                        alItem = (AlertItem)htAlerts[alert.Id]; 
                        lviAlert = alItem.lvItem;

                        if (alItem.monAlert.RepeatCount < alert.RepeatCount)
                        {
                            alItem.itemAge = ItemAge.New;
                            alItem.speakedAlert = false;
                        }
                        else
                        {
                            alItem.itemAge = ItemAge.Old;
                            alItem.speakedAlert = true;
                        }
                        alItem.mouseChecked = false;
                        alItem.monAlert = alert;

                        lviAlert.Text = GetCompiledAlertName(alert);
                    }
                    else
                    {
                        lviAlert = new ListViewItem(GetCompiledAlertName(alert), 5);

                        alItem = new AlertItem(lviAlert, alert, false, nPrevAlerts > 0 ? ItemAge.New : ItemAge.Old,
                                                        nPrevAlerts > 0 ? false : true);
                        lviAlert.Tag = alItem;
                        htAlerts.Add(alert.Id, alItem);

                        lviAlert.SubItems.AddRange(new string[] { "", "", "", "", "", "", "", "" });
                        lvAlerts.Items.Add(lviAlert);

                    }
 
                    lviAlert.SubItems[1].Text = alert.MonitoringObjectInMaintenanceMode? "*" : "";
                    lviAlert.SubItems[2].Text = alert.MonitoringObjectPath;
                    lviAlert.SubItems[3].Text = (alert.Priority != ManagementPackWorkflowPriority.Normal) ?
                                                    alert.Priority.ToString() : "";
                    lviAlert.SubItems[4].Text = alert.Name;
                    lviAlert.SubItems[5].Text = GetDateTimeString(alert.TimeRaised);
                    lviAlert.SubItems[6].Text = GetAgeString(alert.TimeRaised);
                    lviAlert.SubItems[7].Text = (alert.RepeatCount > 0) ? alert.RepeatCount.ToString() : "";
                    lviAlert.SubItems[8].Text = /*(alert.LastModified > alert.TimeAdded) ?*/
                                                        GetDateTimeString(alert.LastModified);
                    //lviAlert.SubItems[9].Text = alert.Owner;

                    ////lvAlerts.Invalidate(lviAlert.Bounds);
                }
                //lvAlerts.Sorting = SortOrder.Ascending;
                //lvAlerts.ListViewItemSorter = new lvAlertComparer();

                lvAlerts.Sort();
                lvAlerts.Invalidate();
            }
            catch (Exception ex)
            {
                LogException("UpdateAlertList", ex);
            }

            Trace.TraceInformation("UpdateAlertList...end");
            return;
        }


        /// <summary>
        /// Build compiled alert computer name
        /// </summary>
        /// <param name="alert">MonitoringAlert</param>
        /// <returns>compiled name</returns>
        private string GetCompiledAlertName(MonitoringAlert alert)
        {
            if (bUseCompiledAlertName && alert.NetbiosComputerName != null && alert.NetbiosComputerName.Length > 1)
                return alert.NetbiosComputerName;
                   
            if (alert.MonitoringObjectDisplayName != null && alert.MonitoringObjectDisplayName.Length > 1)
                return alert.MonitoringObjectDisplayName;
                   
            if (alert.MonitoringObjectPath != null && alert.MonitoringObjectPath.Length > 1)
                return alert.MonitoringObjectPath;

            return alert.Id.ToString();
        }


        /// <summary>
        /// Update Computers list
        /// </summary>
        protected void UpdateComputerList()
        {
            Trace.TraceInformation("UpdateComputerList...start");

            try
            {
                this.tssAll.Text = wndCompObjects.Count.ToString();

                //lvComputers.Enabled = false;
                lvComputers.Clear();
                htComputers.Clear(); 

                int iState;
                short[] arrStat = new short[4] { 0, 0, 0, 0 };
                ListViewItem lviComp;
                foreach (PartialMonitoringObject wndCompObject in wndCompObjects)
                {
                    iState = (int)wndCompObject.HealthState;
                    if (iState < 0 || iState >= arrStat.Length)
                        iState = 0;
                    arrStat[iState]++;

                    if ( (wndCompObject.HealthState==HealthState.Error) && bShowCompStateError 
                        || (wndCompObject.HealthState==HealthState.Warning) && bShowCompStateWarning 
                        || (wndCompObject.HealthState==HealthState.Uninitialized) && bShowCompStateUninitialized
                        || wndCompObject.InMaintenanceMode && bShowCompInMaintenanceMode
                        || wndCompObject.IsAvailable==false)
                    {
                        if (wndCompObject.InMaintenanceMode && bShowCompInMaintenanceMode == false)
                            continue;
                        lviComp = new ListViewItem(wndCompObject.DisplayName, (int)wndCompObject.HealthState);
                        lviComp.Tag = wndCompObject;
                        lvComputers.Items.Add(lviComp);
                        if (wndCompObject.IsAvailable == false && wndCompObject.HealthState!=HealthState.Uninitialized)
                            lviComp.ImageIndex = 4;
                        if (wndCompObject.InMaintenanceMode == true)
                            lviComp.ImageIndex = 7;

                        htComputers.Add(wndCompObject.Id, lviComp);
                    }
                }

                if (lvComputers.Items.Count > 1)
                {
                    Rectangle rcItem = lvComputers.Items[0].GetBounds(ItemBoundsPortion.Entire);
                    int nW = lvComputers.Size.Width / rcItem.Width;
                    int nH = lvComputers.Size.Height / rcItem.Height;

                    int nCompNoOK = htComputers.Count;
                    int needH = nCompNoOK / nW + 1;
                    if (nCompNoOK % nW > 0)
                        needH++;

                    while (needH * rcItem.Height * 5 > this.Height)
                        needH--;

                    if (needH!=nH)
                    {
                        splitContainer1.SplitterDistance -= (needH * rcItem.Height - lvComputers.Size.Height + 5);
                    }

                }

                //lvComputers.Sorting = SortOrder.Ascending;
                //lvComputers.ListViewItemSorter = new lvCompComparer();

                tssCrit.Text = arrStat[(int)HealthState.Error].ToString();
                tssWarn.Text = arrStat[(int)HealthState.Warning].ToString();
                tssOkey.Text = arrStat[(int)HealthState.Success].ToString();
                tssNull.Text = arrStat[(int)HealthState.Uninitialized].ToString();

            }
            catch (Exception ex)
            {
                LogException("UpdateComputerList", ex);
            }

            Trace.TraceInformation("UpdateComputerList...end");
            return;
        }


        #endregion

        #region OwnerDraw ListViewItem (alerts)

        /// <summary>
        /// Draw Item
        /// </summary>
        private void lvAlerts_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            AlertItem alItem = (AlertItem)(e.Item.Tag);

            try
            {
                Color color1 = (e.Item.Index % 2 > 0) ? Color.Beige : Color.White;
                Color color2 = (e.Item.Index % 2 > 0) ? Color.RosyBrown : Color.LightSalmon;
                if (GetAgeInMinutes(alItem.monAlert.LastModified) <= this.EmailAging
                    && alItem.monAlert.ResolvedBy == null && alItem.monAlert.ResolutionState == 0)
                {
                    LinearGradientBrush brush =
                        new LinearGradientBrush(e.Bounds, color1, color2, LinearGradientMode.Horizontal);
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
                else
                {
                    e.Item.BackColor = color1;
                    SolidBrush sBrush = new SolidBrush(color1);
                    e.Graphics.FillRectangle(sBrush, e.Bounds);
                }

                float dGamma = 1.0F;
                if (alItem.itemAge == ItemAge.Deleted)
                    dGamma = 0.2F;
                else 
                    if (alItem.monAlert.Priority == ManagementPackWorkflowPriority.High) 
                        dGamma = 3.5F ;

                ImageAttributes imageAttr = new ImageAttributes();
                imageAttr.SetGamma(dGamma);
                Rectangle rect = new Rectangle(e.Item.Bounds.Location.X, e.Item.Bounds.Location.Y,
                    e.Item.Bounds.Height, e.Item.Bounds.Height);
                //e.Graphics.DrawImage(e.Item.ImageList.Images[e.Item.ImageIndex], rect, 0, 0,
                //    e.Item.Bounds.Height, e.Item.Bounds.Height, GraphicsUnit.Pixel, imageAttr);
                //e.Graphics.DrawImage(e.Item.ImageList.Images[e.Item.ImageIndex], rect);

                Image imgIco = e.Item.ImageList.Images[e.Item.ImageIndex];
                e.Graphics.DrawImage(imgIco, rect, 0, 0,
                    imgIco.Width, imgIco.Width, GraphicsUnit.Pixel, imageAttr);


                e.Item.IndentCount = e.Item.Bounds.Height;

                //e.Graphics.DrawImage(e.Item.ImageList.Images[e.Item.ImageIndex], e.Item.Bounds.Location);
            }
            catch (Exception ex)
            {
                LogException("lvAlerts_DrawItem", ex);
            }

            return; 
        }

        /// <summary>
        /// Draw SubItem
        /// </summary>
        private void lvAlerts_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {

            if (lvAlerts.Columns[e.ColumnIndex].Width == 0)
               return;

            AlertItem alItem = (AlertItem)(e.Item.Tag);
            try
            {
                using (StringFormat sf = new StringFormat())
                {
                    switch (lvAlerts.Columns[e.ColumnIndex].TextAlign)
                    {
                        case HorizontalAlignment.Left:
                            sf.Alignment = StringAlignment.Near;
                            break;
                        case HorizontalAlignment.Center:
                            sf.Alignment = StringAlignment.Center;
                            break;
                        case HorizontalAlignment.Right:
                            sf.Alignment = StringAlignment.Far;
                            break;
                    }

                    Font f1 = (alItem.itemAge == ItemAge.New) ?
                        new Font(e.SubItem.Font, FontStyle.Bold) : e.SubItem.Font;

                    if (alItem.monAlert.Priority == ManagementPackWorkflowPriority.High && alItem.itemAge != ItemAge.Deleted)
                        e.SubItem.ForeColor = Color.Blue;

                    if (alItem.itemAge == ItemAge.Deleted)
                    {
                        e.SubItem.ForeColor = Color.Gray;

                        if (e.ColumnIndex > iColumnAlertName)
                            return;
                        if (e.ColumnIndex == iColumnAlertName)
                        {
                            Rectangle rect2 = new Rectangle(e.Bounds.Right + e.Item.IndentCount, 
                                                            e.Bounds.Top, 5000, e.Bounds.Height);
                            sf.Alignment = StringAlignment.Near;
                            try
                            {
                                alItem.monAlert.Refresh();
                                string sResult = (alItem.monAlert.ResolutionState == 255) ?
                                    "--> " + alItem.monAlert.ResolvedBy : "work -> " + alItem.monAlert.LastModifiedBy;
                                e.Graphics.DrawString(sResult, f1, new SolidBrush(Color.Green), rect2, sf);
                            }
                            catch (Exception ex1)
                            {
                                LogException("lvAlerts_DrawSubItem - Alert Refresh", ex1, false);
                            }
                        }
                    }

                    int xLeft = e.Bounds.X;
                    if (e.ColumnIndex == 0)
                        xLeft += e.Item.IndentCount;
                    int yTop = e.Bounds.Y;

                    int xWidth = e.Bounds.Width; 
                    if (e.ColumnIndex == 0)
                        xWidth -= e.Item.IndentCount;
                    int yHeight = e.Bounds.Height; 

                    Rectangle rect = new Rectangle(xLeft, yTop, xWidth, yHeight);

                    e.Graphics.DrawString(e.SubItem.Text, f1,
                        new SolidBrush(e.SubItem.ForeColor), rect, sf);
                }
            }
            catch (Exception ex)
            {
                LogException("lvAlerts_DrawSubItem", ex);
            }

        }

        /// <summary>
        /// Draw Column Header
        /// </summary>
        private void lvAlerts_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            try
            {
                if (e.Header.Width == 0)
                    return;

                Font headerFont = new Font("Helvetica", iFontColumnHeader, FontStyle.Regular);
                //e.Header.f .Font = headerFont;

                

                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;

                    // Draw the standard header background.
                    e.DrawBackground();

                    // Draw the header text.
                    //using (Font headerFont =  )
                    e.Graphics.DrawString(e.Header.Text, headerFont, Brushes.Black, e.Bounds, sf);
                }
                //e.DrawDefault = true;
            }
            catch (Exception ex)
            {
                LogException("lvAlerts_DrawColumnHeader", ex);
            }

        }

        /// <summary>
        /// Mouse Move
        /// </summary>
        private void lvAlerts_MouseMove(object sender, MouseEventArgs e)
        {
            Trace.TraceInformation("lvAlerts_MouseMove...");

            try
            {
                ListViewItem item = lvAlerts.GetItemAt(e.X, e.Y);

                if (item != null && ((AlertItem)(item.Tag)).mouseChecked == false)
                {
                    lvAlerts.Invalidate(item.Bounds);
                    ((AlertItem)(item.Tag)).mouseChecked = true;
                }
            }
            catch (Exception ex)
            {
                LogException("lvAlerts_MouseMove", ex);
            } 
        }

        /// <summary>
        /// Mouse Up
        /// </summary>
        private void lvAlerts_MouseUp(object sender, MouseEventArgs e)
        {
            Trace.TraceInformation("lvAlerts_MouseUp...");

            try
            {
                ListViewItem clickedItem = this.lvAlerts.GetItemAt(5, e.Y);

                if (clickedItem != null)
                {
                    clickedItem.Selected = true;
                    clickedItem.Focused = true;
                }
            }
            catch (Exception ex)
            {
                LogException("lvAlerts_MouseUp", ex);
            }
            
        }

        #endregion

        #region Misc functions

        /// <summary>
        /// Get long date-time or time only (for today)
        /// </summary>
        /// <param name="dtTime">DateTime</param>
        /// <returns>[ddd, dd.MM ]HH:mm</returns>
        private string GetDateTimeString(DateTime dtTime)
        {
            return dtTime.ToLocalTime().ToString(
                DateTime.Now.ToLocalTime().DayOfYear > dtTime.ToLocalTime().DayOfYear ?
/*"ddd, dd.MM HH:mm"*/     "dd.MM HH:mm" : "HH:mm", CultureInfo.CreateSpecificCulture("ru-RU"));
        }

        /// <summary>
        /// Get age string for DateTime
        /// </summary>
        /// <param name="dtTime">DateTime</param>
        /// <returns>[[D дн.] HH ч.] MM мин.</returns>
        private string GetAgeString(DateTime dtTime)
        {

            TimeSpan tsAge = (DateTime.Now.ToLocalTime() - dtTime.ToLocalTime());

            StringBuilder sAge = new StringBuilder();

            if (tsAge.Days > 0)
                sAge.AppendFormat("{0:d} дн. ", tsAge.Days);

            if (tsAge.Days > 0 || tsAge.Hours > 0)
                sAge.AppendFormat("{0,2:d} ч. ", tsAge.Hours);

            if (tsAge.Days > 0 || tsAge.Hours > 0 || tsAge.Minutes > 0)
                sAge.AppendFormat("{0,2:d} мин.", tsAge.Minutes);
            else
                sAge.Append("< 1 мин.");

            return sAge.ToString();
        }

        /// <summary>
        /// Get age in minutes for DateTime
        /// </summary>
        /// <param name="dtTime"></param>
        /// <returns></returns>
        private double GetAgeInMinutes(DateTime dtTime)
        {
            TimeSpan tsAge = (DateTime.Now.ToLocalTime() - dtTime.ToLocalTime());

            return tsAge.TotalMinutes; 
        }


        /// <summary>
        /// Current Time for clock
        /// </summary>
        /// <returns>H:MM:ss</returns>
        private string GetTimeClock()
        {
            return DateTime.Now.ToLocalTime().ToString("H:mm:ss");
        }


        /// <summary>
        /// Set Status Bar Info Text for long operation (& mouse wait cursor)
        /// </summary>
        /// <param name="sText">Text for info label</param>
        private void SetStatusBarInfo(string sText)
        {

            /*tssInfo.BackColor = SystemColors.Control;
            tssInfo.ForeColor = SystemColors.ControlText; */

            tssInfo.Text = sText + "...";
            ssStatusBar.Update();

            return;
        }


        /// <summary>
        /// Clear Status Bar Info Text after long operation (& set mouse default cursor)
        /// </summary>
        private void ClearStatusBarInfo()
        {

            tssInfo.Text = "";
            ssStatusBar.Update();

            return;
        }

        /// <summary>
        /// Write error to EventLog and to status bar
        /// </summary>
        /// <param name="sFunc"></param>
        /// <param name="ex"></param>
        private void LogException(string sFunc, Exception ex, bool bShowMessage)
        {
            Trace.TraceInformation("LogException({0})...", sFunc);

            StringBuilder sbErr = new StringBuilder();

            sbErr.AppendLine(sFunc);
            sbErr.AppendLine(ex.Message);
            sbErr.AppendLine(ex.StackTrace);

            evtLog.WriteEntry(sbErr.ToString(), EventLogEntryType.Error, 44450);
            //SetStatusBarError(ex.Message);

            if (bShowMessage)
                MessageBoxShowTemp(sbErr.ToString(), "Error!");
                //MessageBox.Show(sbErr.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sText">Message Text</param>
        /// <param name="sCaption">Message Caption</param>
        void MessageBoxShowTemp(string sText, string sCaption)
        {
            MessageBoxShowTemp(sText, sCaption, 15);

            return;
        }

        void MessageBoxShowTemp(string sText, string sCaption, int WaitTimeout)
        {
            Trace.TraceInformation("MessageBoxShowTemp({0})...", sText);

            fErrMessage = new frmErrMsg(WaitTimeout);

            fErrMessage.Text = sCaption;
            fErrMessage.txtMessage.Text = sText;
            fErrMessage.ShowDialog();

            fErrMessage = null;

            return;
        }

        /// <summary>
        /// Write error to EventLog and to status bar
        /// </summary>
        /// <param name="sFunc"></param>
        /// <param name="ex"></param>
        private void LogException(string sFunc, Exception ex)
        {
            LogException(sFunc, ex, true); 
        }

        

        #endregion

        #region Async funcs

        /// <summary>
        /// Main Timer Tick
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {

            if (tssProgBar.Visible)
            {
                tssProgBar.Increment(3);
                if (tssProgBar.Value >= tssProgBar.Maximum)
                    tssProgBar.Value = 0;
            }

            string sTime = GetTimeClock();
            if (sTime != tssTime.Text)
            {
                tssTime.Text = sTime;
            }

            TimeSpan tsAge = DateTime.Now - dtAlertsUpdated;

            if (tsAge.TotalSeconds >= 180 && this.backgroundWorker1.IsBusy == true)
            {
                LogException("timer1_Tick", new Exception("Рестарт консоли (backgroundWorker hung)"), false);
                dtAlertsUpdated = DateTime.Now;
                Application.Restart(); 
            }

            ////if (bCycleOpenWindows && tsAge.TotalSeconds >= (double)UpdInterval && this.backgroundWorker1.IsBusy == false &&
            ////            this.IsConsoleForeground() && tsAge.TotalSeconds < 10000)
            ////{
            ////    
            ////    this.SendToBack();
            ////}

            if (tsAge.TotalSeconds >= (double)UpdInterval && this.backgroundWorker1.IsBusy == false)
            {
                configFile.Refresh();
                if (configFile.LastWriteTime > configTime)
                {
                    configTime = configFile.LastWriteTime;
                    MessageBoxShowTemp("Обновление конфигурации" + Environment.NewLine + configFile.FullName
                                        + Environment.NewLine + configTime, "Информация", 5);
                    System.Threading.Thread.Sleep(5000);
                    Application.Restart(); 
                }

                SimulateUserInput(); 
                SetStatusBarInfo("Загрузка данных: " + appSettings["RMShost"]);
                this.Cursor = Cursors.WaitCursor;
                tssProgBar.Value = 0;
                tssProgBar.Visible = true;

                iProcessedAlertCount = 0;
                this.backgroundWorker1.RunWorkerAsync();

                dtAlertsUpdated = DateTime.Now;
                tssProgBar.PerformStep();

                if (bCycleOpenWindows && tsAge.TotalSeconds < 10000 && (DateTime.Now-dtMoveFG).TotalSeconds>5)
                {
                    Trace.TraceInformation("sending TO FRONT...");
                    this.timer2Wnd.Stop();
                    this.timer2Wnd.Start();
                    dtMoveFG = DateTime.Now;
                    SetConsoleForegroundWnd();
                }

                try
                {
                    if (bShowAcsAgentCount)
                    {
                        int nAcs = (int)perfCntACS.NextValue();
                        if (nAcs > iMaxACS)
                            iMaxACS = nAcs;
                        tssAcs.Text = (nAcs != iMaxACS) ? String.Format("{0}/{1}", nAcs, iMaxACS) : nAcs.ToString();
                    } 
                }
                catch (Exception ex)
                {
                    LogException("perfCntACS", ex, false);
                }
            }
            else
            {
                foreach (ListViewItem lviItem in this.lvAlerts.Items)
                {
                    AlertItem alert = (AlertItem)lviItem.Tag;
                    if (alert.speakedAlert == false)
                    {
                        SpeakAlert(alert);
                        return;
                    }
                }
            }

            return;
        }


        /// <summary>
        /// Cycle windows Timer Tick
        /// </summary>
        private void timer2Wnd_Tick(object sender, EventArgs e)
        {
            if (bCycleOpenWindows == false)
                return;

            TimeSpan tsAge = DateTime.Now - dtAlertsUpdated;

            if (tsAge.TotalSeconds + 10 >= (double)UpdInterval && (DateTime.Now - dtMoveBG).TotalSeconds > 5)
                return;

            dtMoveBG = DateTime.Now;
            Trace.TraceInformation("sending TO BACK...");
            MoveForegroundWindow2Back();
        }


        /// <summary>
        /// Cycle cams Timer Tick
        /// </summary>
        private void timer3Cam_Tick(object sender, EventArgs e)
        {
            if (bCameraPreviewEnable == false)
                return;

            try
            {
                iCamNum = (++iCamNum) % camHosts.Length;

                Trace.TraceInformation("{0}   SHOW CAM --> {1}", DateTime.Now, camHosts[iCamNum]);

                this.txtCamHost.Visible = this.pictCam1.Visible = false;
                txtCamHost.Text = "";
                if (camHosts[iCamNum] != null && camHosts[iCamNum].Length>0)
                {
                    if (ulCam1 != null)
                    {
                        ulCam1.Stop();
                        ulCam1.Reset(); 
                        ulCam1 = null;
                    }
                    ulCam1 = new ULVIEWACTIVEXLib.ULVClass();
                    ulCam1.UserName = sCameraUserName;
                    ulCam1.Password = sCameraPassword;
                    ulCam1.RemotePort = iCameraRemotePort;
                    ulCam1.PreviewHWND = (int)pictCam1.Handle;
                    ulCam1.PreviewHeight = pictCam1.Height;
                    ulCam1.PreviewWidth = pictCam1.Width;
                    ulCam1.RemoteHost = camHosts[iCamNum];
                    
                    int iCamTest = ulCam1.Test(camHosts[iCamNum], ulCam1.RemotePort, ulCam1.UserName, ulCam1.Password);
                    Trace.TraceInformation("Camera {0} test={1}", camHosts[iCamNum], iCamTest);
                    //if (iCamTest == -1)
                    //{
                        ulCam1.Play();
                        this.txtCamHost.Visible = this.pictCam1.Visible = true;
                    //}
                    txtCamHost.Text = ulCam1.RemoteHost;

                    Trace.TraceInformation("{0}   play {1}", DateTime.Now, camHosts[iCamNum]);
                }
            }
            catch (Exception ex0)
            {
                LogException("Camera Preview", ex0, false);
                ulCam1 = null;
            }
        }

        /// <summary>
        /// Start backgroundWorker
        /// </summary>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Trace.TraceInformation("backgroundWorker1_DoWork...start");

            //BackgroundWorker worker = sender as BackgroundWorker;

            LoadScomInfo(0);

            Trace.TraceInformation("backgroundWorker1_DoWork...end");
            return;
        }


        /// <summary>
        /// Finish backgroundWorker
        /// </summary>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Trace.TraceInformation("backgroundWorker1_RunWorkerCompleted...start");

            dtAlertsUpdated = DateTime.Now;

            if (e.Error != null)
            {
                LogException("backgroundWorker1_RunWorkerCompleted", e.Error);
            }
            else
            {
                tssProgBar.PerformStep();
                if (monAlerts != null)
                    UpdateAlertList();

                tssProgBar.PerformStep();
                if (wndCompObjects != null)
                    UpdateComputerList();
            }

            tssProgBar.Value = 100;
            this.Cursor = Cursors.Default;
            tssProgBar.Visible = false;
            ClearStatusBarInfo();

            Trace.TraceInformation("backgroundWorker1_RunWorkerCompleted...end");
            return;
        }


        /// <summary>
        /// Simulate User keyboard Input to block screensaver activation
        /// </summary>
        private void SimulateUserInput()
        {
            Trace.TraceInformation("SimulateUserInput...");
            // Simulate Scroll Lock pressing
                                                                        // Simulate a key press
            SendKeyX.keybd_event((byte)Keys.Scroll, 0, 0, UIntPtr.Zero);
                                                                        // Simulate a key release
            SendKeyX.keybd_event((byte)Keys.Scroll, 0, (uint)SendKeyX.KEYEVENTF_KEYUP, UIntPtr.Zero); 

            //SendKeys.Send("{SCROLLLOCK}");

            //IntPtr iPtr = SendKeyX.GetShellWindow();
            //SendKeyX.SendKey(iPtr, Keys.Scroll);

            // Move Cursor
            /*
            if (Cursor.Position.X<=15 && Cursor.Position.Y<=15)
                Cursor.Position = new Point(400, 400);
            else
                Cursor.Position = new Point(Cursor.Position.X - 10, Cursor.Position.Y - 10);
             * */
        }


        /// <summary>
        /// Set Console Window to Foreground
        /// </summary>
        private void SetConsoleForegroundWnd()
        {
            //string sMsg;

            Trace.TraceInformation("FG={0}", DateTime.Now);


            IntPtr foregroundWindowHandle = SendKeyX.GetForegroundWindow();

            uint remoteThreadId = SendKeyX.GetWindowThreadProcessId(foregroundWindowHandle, IntPtr.Zero);
            Trace.TraceInformation("remoteThreadId={0}...", remoteThreadId);
            //evtLog.WriteEntry(sbErr.ToString(), EventLogEntryType.Error, 44450);

            uint currentThreadId = SendKeyX.GetCurrentThreadId();
            Trace.TraceInformation("currentThreadId={0}...", currentThreadId);

            bool bAlreadyFore = (remoteThreadId == currentThreadId);
            Trace.TraceInformation("...already foreground ={0}", bAlreadyFore);

            //{
            //    Trace.TraceInformation("...already foreground...", currentThreadId);
            //    return;
            //}

            //AttachTrheadInput is needed so we can get the handle of a focused window in another app 
            if (!bAlreadyFore)
                SendKeyX.AttachThreadInput(remoteThreadId, currentThreadId, true);
            //Get the handle of a focused window 
            //IntPtr focused = SendKeyX.GetFocus();
            bool bRes = SendKeyX.SetForegroundWindow(this.Handle);
            Trace.TraceInformation("SetForegroundWindow()={0}", bRes);

            //SendKeyX.SetFocus(this.Handle);
            this.BringToFront();
            //this.UpdateZOrder();
            this.Refresh();

            Trace.TraceInformation("SEND TO FOREGROUND");
            //Now detach since we got the focused handle 
            if (bAlreadyFore)
                SendKeyX.AttachThreadInput(remoteThreadId, currentThreadId, false);

            //Get the text from the active window into the stringbuilder 
            //SendKeyX.SendMessage(focused, SendKeyX.WM_GETTEXT, (IntPtr)builder.Capacity, builder);
            //builder.Append(" Extra text");
            //Change the text in the active window 
            //SendKeyX.SendMessage(focused, SendKeyX.WM_SETTEXT, IntPtr.Zero, builder); 

        }


        /// <summary>
        /// Check if Console Window is Foreground
        /// </summary>
/*        private bool IsConsoleForeground()
        {
            IntPtr foregroundWindowHandle = SendKeyX.GetForegroundWindow();
            uint remoteThreadId = SendKeyX.GetWindowThreadProcessId(foregroundWindowHandle, IntPtr.Zero);
            uint currentThreadId = SendKeyX.GetCurrentThreadId();

            bool bConFore = (remoteThreadId == currentThreadId);
            Trace.TraceInformation("IsConsoleForeground()={0}...", bConFore);

            return bConFore;
        }
*/

        /// <summary>
        /// Move Foreground Window to back
        /// </summary>
        private void MoveForegroundWindow2Back()
        {
            Trace.TraceInformation("Move 2 Back");

            Trace.TraceInformation("BG={0}", DateTime.Now);

            this.SendToBack();
            this.UpdateZOrder();

            //CommandManager.InvalidateRequerySuggested();

            return;

            /*

            IntPtr foregroundWindowHandle = SendKeyX.GetForegroundWindow();
            Trace.TraceInformation("foregroundWindow={0}...", foregroundWindowHandle);

            uint remoteThreadId = SendKeyX.GetWindowThreadProcessId(foregroundWindowHandle, IntPtr.Zero);
            Trace.TraceInformation("remThreadId={0}...", remoteThreadId);

            uint currentThreadId = SendKeyX.GetCurrentThreadId();
            Trace.TraceInformation("curThreadId={0}...", currentThreadId);

            //AttachTrheadInput is needed so we can get the handle of a focused window in another app 
            SendKeyX.AttachThreadInput(remoteThreadId, currentThreadId, true);
            //Get the handle of a focused window 
            IntPtr focused = SendKeyX.GetFocus();
            Trace.TraceInformation("focused={0}...", focused);

            //IntPtr nextWnd = SendKeyX.GetWindow(focused, SendKeyX.GetWindow_Cmd.GW_HWNDNEXT);
            //Trace.TraceInformation("nextWnd={0}...", nextWnd);
            //SendKeyX.SetForegroundWindow(nextWnd);
            //SendKeyX.SetFocus(nextWnd);

            SendKeyX.SendAltTAB(focused); 
           
            //Now detach since we got the focused handle 
            SendKeyX.AttachThreadInput(remoteThreadId, currentThreadId, false);
            */
        }


        #endregion


        private void frmMainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
                ChangeFormBorder();
        }

        private void lvComputers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
                ChangeFormBorder();
        }


        private void ChangeFormBorder()
        {
            if (FormBorderStyle == FormBorderStyle.None)
                FormBorderStyle = FormBorderStyle.Sizable;
            else
                FormBorderStyle = FormBorderStyle.None;

            this.Refresh(); 
        }

        private void ssStatusBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
                ChangeFormBorder();
        }

        private void lvAlerts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem clickedItem = this.lvAlerts.GetItemAt(5, e.Y);

            SpeakAlert ( (AlertItem)clickedItem.Tag);
        }

        private bool ThumbnailAuxCallback()
        {
            return false;
        }

        /// <summary>
        /// Speak Alert Info
        /// </summary>
        /// <param name="alert"></param>
        private void SpeakAlert(AlertItem alert)
        {
            Trace.TraceInformation("SpeakAlert({0})...start", alert.monAlert.Name);

            try
            {
                string sRemoveSuffix = ".x5.ru";

                string compName = alert.monAlert.NetbiosComputerName;
                if (compName == null)
                    compName = alert.monAlert.MonitoringObjectDisplayName;

                if (compName.EndsWith(sRemoveSuffix, StringComparison.OrdinalIgnoreCase) && compName.Length>sRemoveSuffix.Length)
                    compName = compName.Substring(0, compName.Length - sRemoveSuffix.Length);

                iProcessedAlertCount++;

                if (this.bSplashAlert && iProcessedAlertCount <= MaxSplashAlerts)
                {
                    txtAlertSplash.Text = String.Format("{0}\r\n---------------------------\r\n{1}",
                        compName, alert.monAlert.Name);
                    lvAlerts.SendToBack(); 
                    txtAlertSplash.Refresh();
                }

                if (iProcessedAlertCount <= MaxSoundAlerts) // 
                {
                    if (this.sAlarmSound == "system")
                        SystemSounds.Asterisk.Play();
                    else
                        player.PlaySync();
                }

                compName = compName.Replace('-', ' ');
                compName = compName.Replace('.', ' ');

                if (this.bSpeakAlert && alert.monAlert.RepeatCount < 10 && iProcessedAlertCount <= MaxSpeakAlerts
                    && alert.monAlert.Priority == ManagementPackWorkflowPriority.High)
                {
                    PromptBuilder prBuilder = new PromptBuilder();
                    prBuilder.AppendTextWithHint(compName, SayAs.Text);
                    prBuilder.AppendBreak(PromptBreak.Large);
                    prBuilder.AppendText(alert.monAlert.Name);
                    prBuilder.AppendBreak(PromptBreak.ExtraLarge);
                    speaker.Speak(prBuilder);
                }
                else
                {
                    SystemSounds.Asterisk.Play();
                    System.Threading.Thread.Sleep(3000);
                }

                alert.speakedAlert = true;
                if (this.bSplashAlert)
                    txtAlertSplash.SendToBack();
            }
            catch (Exception ex1)
            {
                LogException("SpeakAlert", ex1, false);
            }

            Trace.TraceInformation("SpeakAlert...end");
            return;
        }


    }

    #region AlertItem class

    class AlertItem
    {
        internal ListViewItem lvItem;
        internal MonitoringAlert monAlert;

        internal bool speakedAlert;

        internal bool mouseChecked;
        internal ItemAge itemAge;

        /// <summary>
        /// default constructor
        /// </summary>
        public AlertItem()
        {
            lvItem = null;
            monAlert = null;

            mouseChecked = false;
            itemAge = ItemAge.New;

            speakedAlert = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AlertItem(ListViewItem item, MonitoringAlert alert, bool mchecked, ItemAge age, bool speaked)
        {
            lvItem = item;
            monAlert = alert;

            mouseChecked = mchecked;
            itemAge = age;

            speakedAlert = speaked;
        }

    }

    #endregion


    #region Sorting lists

    /// <summary>
    /// Implements the manual sorting of lvAlerts items by alert.LastModified time
    /// </summary>
    class lvAlertComparer : System.Collections.IComparer
    {
        public lvAlertComparer()
        {
        }

        public int Compare(object x, object y)
        {
            MonitoringAlert alertX = ((AlertItem)(((ListViewItem)x).Tag)).monAlert;
            MonitoringAlert alertY = ((AlertItem)(((ListViewItem)y).Tag)).monAlert;

            if (alertX.Priority != alertY.Priority)
                return (alertX.Priority == ManagementPackWorkflowPriority.High) ? -1 : 1; 

            return DateTime.Compare(alertY.LastModified, alertX.LastModified);
        }
    }

    /// <summary>
    /// Implements the manual sorting of lvComputers items by state & name
    /// </summary>
    class lvCompComparer : System.Collections.IComparer
    {
        public lvCompComparer()
        {
        }

        public int Compare(object x, object y)
        {
            int iStateX=0, iStateY=0;
            PartialMonitoringObject compX = (PartialMonitoringObject)(((ListViewItem)x).Tag);
            PartialMonitoringObject compY = (PartialMonitoringObject)(((ListViewItem)y).Tag);

            if (!compX.IsAvailable && !compX.InMaintenanceMode && compX.HealthState!=HealthState.Uninitialized)
                iStateX = (int)HealthState.Error + 1;
            else 
                iStateX = (int)(compX.InMaintenanceMode ? HealthState.Success : compX.HealthState);

            if (!compY.IsAvailable && !compY.InMaintenanceMode && compY.HealthState != HealthState.Uninitialized)
                iStateY = (int)HealthState.Error + 1;
            else
                iStateY = (int)(compY.InMaintenanceMode ? HealthState.Success : compY.HealthState);

            if (iStateX != iStateY)                         // 1)errors 2)warnings 3)maint/ok 4)not init
                return (iStateY - iStateX);

            if (compX.InMaintenanceMode != compY.InMaintenanceMode)         // 1) maint 2) ok
                return (compY.InMaintenanceMode? 1 : 0) - (compX.InMaintenanceMode? 1 : 0);

            return String.Compare( ((ListViewItem)x).Text, ((ListViewItem)y).Text);     // alphabet sorting
        }
    }


    #endregion


}



