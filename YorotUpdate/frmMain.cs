using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace YorotInstaller
{
    public partial class frmMain : Form
    {
        /// <summary>
        /// General purpose WebClient
        /// </summary>
        private readonly WebClient GPWC = new WebClient();
        private readonly Settings Settings;
        private readonly VersionManager VersionManager;
        private StringEventhHybrid workOn;
        private readonly List<StringEventhHybrid> downloadStrings = new List<StringEventhHybrid>();
        private bool allowClose = true;
        private bool allowSwitch = false;
        private YorotVersion versionToInstall;

        private static string YorotPath => Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Haltroy\\Yorot\\Yorot.exe";

        private static string YorotFolderPath => Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Haltroy\\Yorot\\";

        private static string YorotBetaPath => Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Haltroy\\Yorot\\Yorot Beta.exe";

        private readonly bool YorotExists = File.Exists(YorotPath);
        private readonly bool betaExists = File.Exists(YorotBetaPath);
        #region "Translations"
        private string UIChangeVerMissing = "We couldn't find this version at our archives.";
        private string UIChangeVerArchNotSupported = "Your platform is not supported in this version.";
        private string DownloadProgress = "Downloading [NAME]... [CURRENT]/[TOTAL]";
        private string DownloadYorotDesktop = "Yorot Desktop";
        private string DownloadsComplete = "All downloads are finished.";
        private string InstallComplete = "All installations are finished.";
        private string RegistryComplete = "All registries are registered.";
        private string RegistryStart = "Registering...";
        private string UIYes = "Yes";
        private string UINo = "No";
        private string UIOK = "OK";
        private string UICancel = "Cancel";
        private string UIRepairButton = "Repair";
        private string UIUpdateButton = "Update";
        private string UIInstallVer = "Install [VER]";
        private string UIGatherInfo = "Gathering Information...";
        private string UICheckUpdate = "Checking for updates...";
        private string UIReadyDesc = "Your Yorot is ready to be installed.";
        private string UINotReadyDesc = "You don't meet the requirements for installing Yorot.";
        private string UIChangeVerO1 = "Latest PreOut Version ([PREOUT])";
        private string UIChangeVerO2 = "Latest Stable Version ([LATEST])";
        private string UICreateRecovery = "Creating a restore point...";
        private string UIDoneUninstall = "Yorot successfully uninstalled." + Environment.NewLine + "" + Environment.NewLine + "It's improtant for us to listen to your reason why you decided to uninstall Yorot so please open an issue in GitHub by clickng &quot;Send Feedback&quot; button below." + Environment.NewLine + "" + Environment.NewLine + "Farewell, old firend." + Environment.NewLine + "" + Environment.NewLine + "";
        private string UIDoneInstall = "Yorot installed successfully." + Environment.NewLine + "" + Environment.NewLine + "Closing this program will start the application.";
        private string UIDoneUpdate = "Yorot updated successfully.";
        private string UIDoneRepair = "Yorot repaired successfully.";
        private string UIDoneError = "An error occured while doing your request. Don't worry, we restored your Yorot installation. Please create an issue on GitHub by clicking &quot;Send Feedback&quot; and copy-paste this information below:";
        private string UIPreOutAvailable = "You meet the requirements.";
        private string UIPreOutDisable = "You don't meet the requirements. Update or Repair your Yorot first.";
        private string UICreateShortcut = "Creating shortcuts...";
        private string UIUpdating = "Updating Installer... Please wait..." + Environment.NewLine + "[PERC]% | [CURRENT] KiB downloaded out of [TOTAL] KiB.";

        public void LoadLang()
        {
            UIInstallVer = Settings.GetItemText("UIInstallVer");
            lbChangeVerDesc.Text = Settings.GetItemText("UIChangeVerDesc");
            UIChangeVerMissing = Settings.GetItemText("UIChangeVerMissing");
            UIChangeVerArchNotSupported = Settings.GetItemText("UIChangeVerArchNotSupported");
            DownloadProgress = Settings.GetItemText("DownloadProgress");
            DownloadYorotDesktop = Settings.GetItemText("DownloadYorotDesktop");
            DownloadsComplete = Settings.GetItemText("DownloadsComplete");
            InstallComplete = Settings.GetItemText("InstallComplete");
            RegistryComplete = Settings.GetItemText("RegistryComplete");
            RegistryStart = Settings.GetItemText("RegistryStart");
            UIYes = Settings.GetItemText("UIYes");
            UINo = Settings.GetItemText("UINo");
            UIOK = Settings.GetItemText("UIOK");
            UICancel = Settings.GetItemText("UICancel");
            btClose.Text = Settings.GetItemText("UIClose");
            UIGatherInfo = Settings.GetItemText("UIGatherInfo");
            UICheckUpdate = Settings.GetItemText("UICheckUpdate");
            UIReadyDesc = Settings.GetItemText("UIReadyDesc");
            UINotReadyDesc = Settings.GetItemText("UINotReadyDesc");
            lbModifyDesc.Text = Settings.GetItemText("UIModifyDesc");
            btInstall.Text = Settings.GetItemText("UIReadyButton");
            UIRepairButton = Settings.GetItemText("UIRepairButton");
            UIUpdateButton = Settings.GetItemText("UIUpdateButton");
            btUninstall.Text = Settings.GetItemText("UIUninstallButton");
            btChangeVer.Text = Settings.GetItemText("UIChangeVerButton");
            UIChangeVerO1 = Settings.GetItemText("UIChangeVerO1");
            UIChangeVerO2 = Settings.GetItemText("UIChangeVerO2");
            rbOld.Text = Settings.GetItemText("UIChangeVerO3");
            lbDownloading.Text = Settings.GetItemText("UIDownloading");
            lbInstalling.Text = Settings.GetItemText("UIInstalling");
            UICreateRecovery = Settings.GetItemText("UICreateRecovery");
            UIDoneUninstall = Settings.GetItemText("UIDoneUninstall");
            UIDoneInstall = Settings.GetItemText("UIDoneInstall");
            UIDoneUpdate = Settings.GetItemText("UIDoneUpdate");
            UIDoneRepair = Settings.GetItemText("UIDoneRepair");
            UIDoneError = Settings.GetItemText("UIDoneError");
            btSendFeedback.Text = Settings.GetItemText("UISendFeedback");
            UIPreOutAvailable = Settings.GetItemText("UIPreOutAvailable");
            UIPreOutDisable = Settings.GetItemText("UIPreOutDisable");
            lbVersionToInstall.Text = Settings.GetItemText("UIVersionToInstall");
            UICreateShortcut = Settings.GetItemText("CreateShortcut");
            UIUpdating = Settings.GetItemText("UIUpdating");
            if (VersionManager.PreOutVerNumber != 0 && VersionManager.LatestVersionNumber != 0)
            {
                rbPreOut.Text = UIChangeVerO1.Replace("[PREOUT]", VersionManager.GetVersionFromVersionNo(VersionManager.PreOutVerNumber).VersionText);
                rbStable.Text = UIChangeVerO2.Replace("[LATEST]", VersionManager.GetVersionFromVersionNo(VersionManager.LatestVersionNumber).VersionText);
                if (supportsLatestPreOut())
                {
                    lbPerOutReq.Text = UIPreOutAvailable;
                    rbPreOut.Enabled = true;
                }
                else
                {
                    lbPerOutReq.Text = UIPreOutDisable;
                    rbPreOut.Enabled = false;
                }
            }
            if (VersionManager.Versions.Count > 0 && YorotExists)
            {
                YorotVersion current = VersionManager.GetVersionFromVersionName(FileVersionInfo.GetVersionInfo(YorotExists ? YorotPath : YorotBetaPath).ProductVersion);
                btRepair.Text = current != null
                    ? VersionManager.LatestVersionNumber != current.VersionNo || VersionManager.PreOutVerNumber != current.VersionNo
                        ? UIUpdateButton
                        : UIRepairButton
                    : UIRepairButton;
            }
            switch (DoneType)
            {
                case DoneType.Install:
                    lbDoneDesc.Text = UIDoneInstall;
                    break;
                case DoneType.Repair:
                    lbDoneDesc.Text = UIDoneRepair;
                    break;
                case DoneType.Update:
                    lbDoneDesc.Text = UIDoneUpdate;
                    break;
                case DoneType.Uninstall:
                    lbDoneDesc.Text = UIDoneUninstall;
                    break;
            }
            label4.Text = isUpdatingInstaller ? UIUpdating.Replace("[NAME]", workOn.String3).Replace("[PERC]", "" + updatePerc).Replace("[CURRENT]", updateCurrent).Replace("[TOTAL]", updateTotal) : UIGatherInfo;
            btInstall1.Text = UIInstallVer.Replace("[VER]", versionToInstall != null ? versionToInstall.VersionText : "");
            lbReady.Text = canInstall ? UIReadyDesc : UINotReadyDesc;
            cbOld.Location = new Point(lbVersionToInstall.Location.X + lbVersionToInstall.Width, cbOld.Location.Y);
        }

        #endregion "Translations"
        public frmMain(Settings settings)
        {
            Settings = settings;
            VersionManager = new VersionManager();
            isShiftPressed = false;
            isUpdatingInstaller = false;
            reqs = new List<PreResqs.PreResq>();
            InitializeComponent();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            GPWC.DownloadStringCompleted += GPWC_DownloadStringComplete;
            GPWC.DownloadProgressChanged += GPWC_ProgressChanged;
            GPWC.DownloadFileCompleted += GPWC_DownloadFileComplete;
            string[] langFiles = Directory.GetFiles(Settings.WorkFolder, "*.language");
            DoneType = DoneType.Install;
            cbLang.Items.Clear();
            for (int i = 0; i < langFiles.Length; i++)
            {
                cbLang.Items.Add(Path.GetFileNameWithoutExtension(langFiles[i]));
            }
            lbDoneDesc.MaximumSize = new Size(tbDoneError.Width, 0);
            cbLang.Text = Path.GetFileNameWithoutExtension(Settings.LanguageFile);
            LoadLang();
            updateTheme();
        }

        private bool isUpdatingInstaller;
        private int updatePerc;
        private string updateCurrent;
        private string updateTotal;

        private void GPWC_DownloadStringComplete(object sender, DownloadStringCompletedEventArgs e)
        {
            if (GPWC.IsBusy) { GPWC.CancelAsync(); }
            if (workOn.Type == StringEventhHybrid.StringType.String)
            {
                workOn.RunEvent(this, e);
                downloadStrings.Remove(workOn);
                workOn = downloadStrings.Find(i => i.Type == StringEventhHybrid.StringType.String);
                if (workOn != null)
                {
                    GPWC.DownloadStringAsync(new Uri(workOn.String));
                }
                else
                {
                    GPWC_AllJobsDone();
                }
            }
        }

        private int installJobCount = 0;
        private int installDoneCount = 0;
        private bool isPreparing = true;
        private int doneCount = 0;
        private void GPWC_ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (workOn.Type == StringEventhHybrid.StringType.File && !isPreparing)
            {
                Invoke(new Action(() =>
                {
                    lbDownloadInfo.Text = DownloadProgress.Replace("[NAME]", workOn.String3).Replace("[PERC]", "" + e.ProgressPercentage).Replace("[CURRENT]", (e.BytesReceived / 1024) + " KiB").Replace("[TOTAL]", (e.TotalBytesToReceive / 1024) + " KiB");
                    int downloadTotoalJobs = downloadStrings.FindAll(i => i.Type == StringEventhHybrid.StringType.File).Count;
                    lbDownloadCount.Text = (doneCount == downloadTotoalJobs ? doneCount : doneCount + 1) + "/" + downloadTotoalJobs;
                    pbDownload.Width = e.ProgressPercentage * (pDownload.Width / 100);
                }));
            }
            if (isUpdatingInstaller)
            {
                Invoke(new Action(() =>
                {
                    updatePerc = e.ProgressPercentage;
                    updateCurrent = (e.BytesReceived / 1024) + " KiB";
                    updateTotal = (e.TotalBytesToReceive / 1024) + " KiB";
                    label4.Text = UIUpdating.Replace("[NAME]", workOn.String3).Replace("[PERC]", "" + e.ProgressPercentage).Replace("[CURRENT]", (e.BytesReceived / 1024) + " KiB").Replace("[TOTAL]", (e.TotalBytesToReceive / 1024) + " KiB");
                }));
            }
        }




        private void GPWC_DownloadFileComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (GPWC.IsBusy) { GPWC.CancelAsync(); }
            if (workOn.Type == StringEventhHybrid.StringType.File)
            {
                workOn.RunEvent(this, e);
                downloadStrings.Remove(workOn);
                workOn = downloadStrings.Find(i => i.Type == StringEventhHybrid.StringType.File);
                doneCount++;
                if (workOn != null)
                {
                    GPWC.DownloadFileAsync(new Uri(workOn.String), workOn.String2);
                }
                else
                {
                    GPWC_AllJobsDone();
                    if (!isPreparing)
                    {
                        lbDownloadInfo.Text = DownloadsComplete;
                    }
                }
            }
        }
        private void updateInstaller(object sender, EventArgs E)
        {
            if (E is AsyncCompletedEventArgs)
            {
                AsyncCompletedEventArgs e = E as AsyncCompletedEventArgs;
                if (e.Error != null)
                {
                    Error(new Exception("Error while updating Installer. Error: " + e.ToString()));
                }
                else if (e.Cancelled)
                {
                    Error(new Exception("Error while updating Installer. Error: Cancelled."));
                }
                else
                {
                    allowClose = true;
                    Settings.Save();
                    Process.Start(new ProcessStartInfo(Settings.WorkFolder + "YorotInstaller.exe") { UseShellExecute = true, Verb = "runas" });
                    Application.Exit();
                }
            }
            else
            {
                Error(new Exception("\"E\" is not an AsyncCompletedEventArgs [in void \"updateInstaller\"]."));
            }
        }

        private bool canInstall;

        private void GPWC_AllJobsDone()
        {
            if (downloadStrings.Count > 0 && !GPWC.IsBusy && workOn == null)
            {
                if (downloadStrings[0].Type == StringEventhHybrid.StringType.File)
                {
                    DoFileWork(downloadStrings[0]);
                }
                else
                {
                    DoStringWork(downloadStrings[0]);
                }
                return;
            }
            if (isPreparing) // TODO: Add Update checking & update Installer if available, then release it
            {
                if (VersionManager.InstallerVer < VersionManager.LatestInstallerVer)
                {
                    isUpdatingInstaller = true;
                    allowClose = false;
                    StringEventhHybrid seh = new StringEventhHybrid()
                    {
                        String = "https://haltroy.com/YorotInstaller.html",
                        String2 = Settings.WorkFolder + "YorotInstaller.exe",
                        String3 = "Installer",
                        Type = StringEventhHybrid.StringType.File,
                    };
                    seh.Event += updateInstaller;
                    DoFileWork(seh);
                    return;
                }
                isPreparing = false;
                allowSwitch = true;
                if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Haltroy\\Yorot\\Yorot.exe") && !File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Haltroy\\Yorot\\Yorot Beta.exe"))
                {
                    allowSwitch = true;
                    tabControl1.SelectedTab = tpFirst;
                    if (supportsThisVer(VersionManager.GetVersionFromVersionNo(VersionManager.LatestVersionNumber)))
                    {
                        canInstall = true;
                        lbReady.Text = UIReadyDesc;
                        btInstall.Enabled = true;
                        btInstall.Visible = true;
                    }
                    else
                    {
                        canInstall = false;
                        lbReady.Text = UINotReadyDesc;
                        btInstall.Enabled = false;
                        btInstall.Visible = false;
                    }
                }
                else
                {
                    allowSwitch = true;
                    tabControl1.SelectedTab = tpModify;
                    if (VersionManager.Versions.Count > 0)
                    {
                        YorotVersion current = VersionManager.GetVersionFromVersionName(FileVersionInfo.GetVersionInfo(YorotExists ? YorotPath : YorotBetaPath).ProductVersion);
                        btRepair.Text = current != null
                            ? VersionManager.LatestVersionNumber != current.VersionNo || VersionManager.PreOutVerNumber != current.VersionNo
                                ? UIUpdateButton
                                : UIRepairButton
                            : UIRepairButton;
                    }
                }
            }
            else
            {
                lbDownloadInfo.Text = DownloadsComplete;
                pbDownload.Width = pDownload.Width;
            }
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            StringEventhHybrid htupdate = new StringEventhHybrid() { String = "https://raw.githubusercontent.com/Haltroy/Yorot/master/Yorot.htupdate", Type = StringEventhHybrid.StringType.String, };
            htupdate.Event += htupdateDownloaded;
            DoStringWork(htupdate);
        }

        private void DoStringWork(StringEventhHybrid seh)
        {
            if (!downloadStrings.Contains(seh))
            {
                downloadStrings.Add(seh);
            }
            if (!GPWC.IsBusy) { workOn = seh; GPWC.DownloadStringAsync(new Uri(seh.String)); }
        }

        private void DoFileWork(StringEventhHybrid seh)
        {
            if (!downloadStrings.Contains(seh))
            {
                downloadStrings.Add(seh);
            }
            if (!GPWC.IsBusy) { workOn = seh; GPWC.DownloadFileAsync(new Uri(seh.String), seh.String2); }
        }

        private DoneType DoneType;

        private void Successful()
        {
            timer1.Stop();
            allowClose = true;
            allowSwitch = true;
            tabControl1.SelectedTab = tpDone;
            switch (DoneType)
            {
                case DoneType.Install:
                    lbDoneDesc.Text = UIDoneInstall;
                    break;
                case DoneType.Repair:
                    lbDoneDesc.Text = UIDoneRepair;
                    break;
                case DoneType.Update:
                    lbDoneDesc.Text = UIDoneUpdate;
                    break;
                case DoneType.Uninstall:
                    lbDoneDesc.Text = UIDoneUninstall;
                    break;
            }
            tbDoneError.Visible = false;
            tbDoneError.Enabled = false;
        }

        private void htupdateDownloaded(object sender, EventArgs E)
        {
            if (E is DownloadStringCompletedEventArgs)
            {
                DownloadStringCompletedEventArgs e = E as DownloadStringCompletedEventArgs;
                if (e.Error == null && !e.Cancelled)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(e.Result);
                    XmlNode firstnode = doc.FirstChild.Name != "HaltroyUpdate" ? doc.FirstChild.NextSibling : doc.FirstChild;
                    int workCount = 0;
                    foreach (XmlNode node in firstnode.ChildNodes)
                    {
                        if (node.Name == "PreOutVer")
                        {
                            VersionManager.LatesPreOut = node.InnerXml;
                            workCount++;
                        }
                        else if (node.Name == "PreOutNo")
                        {
                            VersionManager.PreOutVerNumber = Convert.ToInt32(node.InnerXml);
                            workCount++;
                        }
                        else if (node.Name == "PreOutLow")
                        {
                            VersionManager.PreOutMinVer = Convert.ToInt32(node.InnerXml);
                            workCount++;
                        }
                        else if (node.Name == "AppVersionNo")
                        {
                            VersionManager.LatestVersionNumber = Convert.ToInt32(node.InnerXml);
                            workCount++;
                        }
                        else if (node.Name == "MinimumNo")
                        {
                            VersionManager.LatestUpdateMinVer = Convert.ToInt32(node.InnerXml);
                            workCount++;
                        }
                        else if (node.Name == "InstallerVer")
                        {
                            VersionManager.LatestInstallerVer = Convert.ToInt32(node.InnerXml);
                            workCount++;
                        }
                        else if (node.Name == "AppVersion")
                        {
                            VersionManager.LatesVersion = node.InnerXml;
                            workCount++;
                        }
                        else if (node.Name == "Versions")
                        {
                            foreach (XmlNode subnode in node.ChildNodes)
                            {
                                if (subnode.Name == "Version")
                                {
                                    if (subnode.Attributes["VersionNo"] != null && subnode.Attributes["Flags"] != null && subnode.Attributes["Text"] != null)
                                    {
                                        if (subnode.Attributes["Flags"].Value.Contains("missing"))
                                        {
                                            YorotVersion ver = new YorotVersion(subnode.Attributes["Text"].Value, Convert.ToInt32(subnode.Attributes["VersionNo"].Value), subnode.Attributes["Flags"].Value);
                                            VersionManager.Versions.Add(ver);
                                        }
                                        else
                                        {
                                            YorotVersion ver = new YorotVersion(subnode.Attributes["Text"].Value, Convert.ToInt32(subnode.Attributes["VersionNo"].Value), subnode.Attributes["ZipPath"].Value, subnode.Attributes["Flags"].Value, subnode.Attributes["Reg"].Value);
                                            VersionManager.Versions.Add(ver);
                                        }
                                    }
                                }
                            }
                            workCount++;
                        }
                    }
                }
                else
                {
                    StringEventhHybrid htupdate = new StringEventhHybrid() { String = "https://raw.githubusercontent.com/Haltroy/Yorot/master/Yorot.htupdate", Type = StringEventhHybrid.StringType.String, };
                    htupdate.Event += htupdateDownloaded;
                }
            }
            else
            {
                Console.WriteLine(" [HTUPDATE] Error: EventArgs is not suitable.");
            }
        }


        private void label3_Click(object sender, EventArgs e)
        {
            if (allowClose)
            {
                Settings.Save();
                Close();
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e);
        }


        private void updateTheme()
        {
            BackColor = Settings.BackColor;
            ForeColor = Settings.ForeColor;
            cbLang.ForeColor = Settings.MidColor;
            btInstall.ForeColor = Settings.ForeColor;
            btInstall1.ForeColor = Settings.ForeColor;
            cbOld.ForeColor = Settings.ForeColor;
            pChangeVer.ForeColor = Settings.ForeColor;
            btRepair.ForeColor = Settings.ForeColor;
            btUninstall.ForeColor = Settings.ForeColor;
            btChangeVer.ForeColor = Settings.ForeColor;
            btClose.ForeColor = Settings.ForeColor;
            btSendFeedback.ForeColor = Settings.ForeColor;
            tbDoneError.ForeColor = Settings.ForeColor;
            panel1.ForeColor = Settings.ForeColor;

            pictureBox2.Image = Settings.isDarkMode ? Properties.Resources.dark : Properties.Resources.light;
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                TabPage page = tabControl1.TabPages[i];
                page.BackColor = Settings.BackColor1;
                page.ForeColor = Settings.ForeColor;
            }

            BackColor = Settings.BackColor;
            pDownload.BackColor = Settings.BackColor2;
            panel1.BackColor = Settings.BackColor;
            pInstall.BackColor = Settings.BackColor2;
            pChangeVer.BackColor = Settings.BackColor2;
            flowLayoutPanel1.BackColor = Settings.BackColor1;
            tbDoneError.BackColor = Settings.BackColor2;
            cbOld.BackColor = Settings.BackColor3;
            btInstall.BackColor = Settings.BackColor2;
            btRepair.BackColor = Settings.BackColor2;
            btUninstall.BackColor = Settings.BackColor2;
            btChangeVer.BackColor = Settings.BackColor2;
            btInstall1.BackColor = Settings.BackColor3;
            cbLang.BackColor = Settings.BackColor1;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Settings.isDarkMode = !Settings.isDarkMode;
            updateTheme();
        }

        private bool isBusy = false;
        private readonly List<PreResqs.PreResq> reqs;
        private PreResqs.PreResq workingReq;
        private async Task<bool> InstallPreResq(PreResqs.PreResq req)
        {
            await Task.Run(() =>
            {
                if (isBusy)
                {
                    reqs.Add(req);
                    return;
                }
                isBusy = true;
                workingReq = req;
                ProcessStartInfo info = new ProcessStartInfo(Settings.WorkFolder + req.FileName, req.SilentArgs) { UseShellExecute = true, Verb = "runas" };
                Process process = new Process
                {
                    StartInfo = info
                };
                process.Start();
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    isBusy = false;
                    if (reqs.Contains(req)) { reqs.Remove(req); }
                    if (reqs.Count > 0)
                    {
                        workingReq = reqs[0];
                        Task.Run(() => InstallPreResq(workingReq));
                    }
                    else
                    {
                        workingReq = null;
                    }
                }
                else
                {
                    Error(new Exception("Required component \"" + req.Name + "\" is not installed. Exit Code: " + process.ExitCode));
                }
            });
            return false;
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (allowSwitch) { allowSwitch = false; } else { e.Cancel = true; }
        }

        private void cbLang_DropDown(object sender, EventArgs e)
        {
            string[] langFiles = Directory.GetFiles(Settings.WorkFolder, "*.language");
            cbLang.Items.Clear();
            for (int i = 0; i < langFiles.Length; i++)
            {
                cbLang.Items.Add(Path.GetFileNameWithoutExtension(langFiles[i]));
            }
        }

        private void cbLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.LanguageFile = Settings.WorkFolder + cbLang.SelectedItem.ToString() + ".language";
            Settings.LoadLang(Settings.WorkFolder + cbLang.SelectedItem.ToString() + ".language");
            LoadLang();
        }
        private bool supportsLatestPreOut()
        {

            if (!YorotExists && !betaExists)
            {
                return false;
            }
            YorotVersion vPreOut = VersionManager.GetVersionFromVersionNo(VersionManager.PreOutVerNumber);
            if (vPreOut.isOnlyx64 & !PreResqs.is64BitMachine) { return false; }
            YorotVersion currentVer = VersionManager.GetVersionFromVersionName(FileVersionInfo.GetVersionInfo(YorotExists ? YorotPath : YorotBetaPath).ProductVersion);
            if (currentVer == null) { rbPreOut.Checked = true; return true; }
            if (currentVer.VersionNo < VersionManager.LatestVersionNumber || currentVer.VersionNo < VersionManager.PreOutMinVer) { return false; }
            return true;
        }

        private bool supportsThisVer(YorotVersion ver)
        {
            if (ver.isOnlyx64 && !PreResqs.is64BitMachine) { return false; }
            if (ver.RequiresNet452 && !PreResqs.SystemSupportsNet452) { return false; }
            if (ver.RequiresNet461 && !PreResqs.SystemSupportsNet461) { return false; }
            if (ver.RequiresNet48 && !PreResqs.SystemSupportsNet48) { return false; }
            if (ver.RequiresVisualC2015 && !PreResqs.SystemSupportsVisualC2015x86) { return false; }
            return true;
        }

        private void Error(Exception ex)
        {
            allowClose = true;
            allowSwitch = true;
            reqs.Clear();
            downloadStrings.Clear();
            if (GPWC.IsBusy) { GPWC.CancelAsync(); }
            workOn = null;
            tabControl1.SelectedTab = tpDone;
            lbDoneDesc.Text = UIDoneError;
            tbDoneError.Text = ex.ToString();
            tbDoneError.Location = new Point(tbDoneError.Location.X, lbDoneDesc.Location.Y + lbDoneDesc.Height);
            tbDoneError.Height = Height - (panel1.Height + flowLayoutPanel1.Height + lbDoneDesc.Height + 35);
        }

        private void appShortcut()
        {
            HTAltTools.appShortcut(YorotExists ? YorotPath : YorotBetaPath, Environment.GetFolderPath(Environment.SpecialFolder.Programs) + "\\" + "Yorot");
            HTAltTools.appShortcut(YorotExists ? YorotPath : YorotBetaPath, Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms) + "\\" + "Yorot");
            if (versionToInstall.VersionNo >= 36)
            {
                HTAltTools.appShortcut(YorotExists ? YorotPath : YorotBetaPath, Environment.GetFolderPath(Environment.SpecialFolder.Programs) + "\\" + "Yorot", "--make-ext");
                HTAltTools.appShortcut(YorotExists ? YorotPath : YorotBetaPath, Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms) + "\\" + "Yorot", "--make-ext");
            }
            installDoneCount++;
        }

        private void StartInstallation(bool forceReqs = false)
        {
            allowSwitch = true;
            allowClose = false;
            tabControl1.SelectedTab = tpProgress;
            if (versionToInstall.isMissing) { Error(new Exception("This version \"" + versionToInstall.ToString() + "\" is missing.")); }
            if (versionToInstall.RequiresNet452 && !PreResqs.SystemSupportsNet452) { Error(new Exception("This version \"" + versionToInstall.ToString() + "\" requires .NEt Framework 4.5.2 but your computer does not supports it.")); }
            if (versionToInstall.RequiresNet461 && !PreResqs.SystemSupportsNet461) { Error(new Exception("This version \"" + versionToInstall.ToString() + "\" requires .NEt Framework 4.6.1 but your computer does not supports it.")); }
            if (versionToInstall.RequiresNet48 && !PreResqs.SystemSupportsNet48) { Error(new Exception("This version \"" + versionToInstall.ToString() + "\" requires .NEt Framework 4.8 but your computer does not supports it.")); }
            if (versionToInstall.RequiresVisualC2015 && !PreResqs.SystemSupportsVisualC2015x86) { Error(new Exception("This version \"" + versionToInstall.ToString() + "\" requires Visual C++ 2015 but your computer does not supports it.")); }
            if (versionToInstall.isOnlyx64 && !PreResqs.is64BitMachine) { Error(new Exception("This version \"" + versionToInstall.ToString() + "\" requires 64-bit machine but this machine is not a 64 bit machine.")); }
            isPreparing = false;
            if (versionToInstall.RequiresNet452 && (!PreResqs.isInstalled(PreResqs.NetFramework452) || forceReqs))
            {
                StringEventhHybrid seh = new StringEventhHybrid()
                {
                    String = PreResqs.NetFramework452.Url,
                    String2 = Settings.WorkFolder + PreResqs.NetFramework452.FileName,
                    String3 = PreResqs.NetFramework452.Name,
                    Type = StringEventhHybrid.StringType.File,
                };
                seh.Event += new StringEventhHybrid.EventDelegate((sender, e) => { Task.Run(() => InstallPreResq(PreResqs.NetFramework452)); });
                installJobCount++;
                downloadStrings.Add(seh);
            }
            if (versionToInstall.RequiresNet461 && (!PreResqs.isInstalled(PreResqs.NetFramework461) || forceReqs))
            {
                StringEventhHybrid seh = new StringEventhHybrid()
                {
                    String = PreResqs.NetFramework461.Url,
                    String2 = Settings.WorkFolder + PreResqs.NetFramework461.FileName,
                    String3 = PreResqs.NetFramework461.Name,
                    Type = StringEventhHybrid.StringType.File,
                };
                seh.Event += new StringEventhHybrid.EventDelegate((sender, e) => { Task.Run(() => InstallPreResq(PreResqs.NetFramework461)); });
                installJobCount++;
                downloadStrings.Add(seh);
            }
            if (versionToInstall.RequiresNet48 && (!PreResqs.isInstalled(PreResqs.NetFramework48) || forceReqs))
            {
                StringEventhHybrid seh = new StringEventhHybrid()
                {
                    String = PreResqs.NetFramework48.Url,
                    String2 = Settings.WorkFolder + PreResqs.NetFramework48.FileName,
                    String3 = PreResqs.NetFramework48.Name,
                    Type = StringEventhHybrid.StringType.File,
                };
                seh.Event += new StringEventhHybrid.EventDelegate((sender, e) => { Task.Run(() => InstallPreResq(PreResqs.NetFramework48)); });
                installJobCount++;
                downloadStrings.Add(seh);
            }
            if (versionToInstall.RequiresVisualC2015 && (!PreResqs.isInstalled(PreResqs.VisualC2015x86) || forceReqs))
            {

                StringEventhHybrid seh = new StringEventhHybrid()
                {
                    String = PreResqs.VisualC2015x86.Url,
                    String2 = Settings.WorkFolder + PreResqs.VisualC2015x86.FileName,
                    String3 = PreResqs.VisualC2015x86.Name,
                    Type = StringEventhHybrid.StringType.File,
                };
                seh.Event += new StringEventhHybrid.EventDelegate((sender, e) => { Task.Run(() => InstallPreResq(PreResqs.VisualC2015x86)); });
                installJobCount++;
                downloadStrings.Add(seh);

                if (PreResqs.is64BitMachine && (!PreResqs.isInstalled(PreResqs.VisualC2015x64) || forceReqs))
                {
                    StringEventhHybrid seh2 = new StringEventhHybrid()
                    {
                        String = PreResqs.VisualC2015x64.Url,
                        String2 = Settings.WorkFolder + PreResqs.VisualC2015x64.FileName,
                        String3 = PreResqs.VisualC2015x64.Name,
                        Type = StringEventhHybrid.StringType.File,
                    };
                    seh.Event += new StringEventhHybrid.EventDelegate((sender, e) => { Task.Run(() => InstallPreResq(PreResqs.VisualC2015x64)); });
                    installJobCount++;
                    downloadStrings.Add(seh2);
                }
            }
            StringEventhHybrid sehK = new StringEventhHybrid()
            {
                String = versionToInstall.ZipPath.Replace("[VERSION]", versionToInstall.VersionText).Replace("[ARCH]", PreResqs.is64BitMachine ? "x64" : "x86").Replace("[U]", "F"),
                String2 = Settings.WorkFolder + versionToInstall.VersionText + ".htpackage",
                String3 = DownloadYorotDesktop,
                Type = StringEventhHybrid.StringType.File,
            };
            sehK.Event += installYorot;
            installJobCount++;
            downloadStrings.Add(sehK);
            if (workOn == null)
            {
                workOn = downloadStrings[0];
            }
            timer1.Start();
            installJobCount += 3;
            DoFileWork(workOn);
            Task.Run(() => GetBackup());
            lbInstallInfo.Text = UICreateRecovery;
        }

        private async Task<bool> GetBackup(bool retrieve = false)
        {
            await Task.Run(() =>
            {
                string backupLocation = Settings.WorkFolder + "backup.htpackage";

                if (!retrieve)
                {
                    if (YorotExists)
                    {
                        if (File.Exists(backupLocation)) { File.Delete(backupLocation); }
                        ZipFile.CreateFromDirectory(YorotFolderPath, backupLocation, CompressionLevel.NoCompression, false, Encoding.UTF8);
                    }
                    installDoneCount++;
                }
                else
                {
                    if (File.Exists(backupLocation))
                    {
                        if (new FileInfo(backupLocation).Length > 0)
                        {
                            if (File.Exists(YorotPath) || File.Exists(YorotBetaPath)) { Directory.Delete(YorotFolderPath, true); }
                            Directory.CreateDirectory(YorotFolderPath);
                            ZipFile.ExtractToDirectory(backupLocation, YorotFolderPath, Encoding.UTF8);
                        }
                    }
                }
            });
            return true;
        }

        private void installYorot(object sender, EventArgs E)
        {
            if (E is AsyncCompletedEventArgs)
            {
                AsyncCompletedEventArgs e = E as AsyncCompletedEventArgs;
                if (e.Error != null)
                {
                    Error(new Exception("Error while downloading Yorot Desktop files [\"" + versionToInstall.ZipPath.Replace("[VERSION]", versionToInstall.VersionText).Replace("[ARCH]", PreResqs.is64BitMachine ? "x64" : "x86").Replace("[U]", "F") + "\"]. Error: " + e.Error.ToString()));
                }
                else if (e.Cancelled)
                {
                    Error(new Exception("Error while downloading Yorot Desktop files [\"" + versionToInstall.ZipPath.Replace("[VERSION]", versionToInstall.VersionText).Replace("[ARCH]", PreResqs.is64BitMachine ? "x64" : "x86").Replace("[U]", "F") + "\"]. Error: Cancelled."));
                }
                else
                {
                    Task.Run(() => InstallYorotFoReal());
                }
            }
            else
            {
                Error(new Exception("\"E\" is not an AsyncCompletedEventArgs (in void \"installYorot\")."));
            }
        }

        /// <summary>
        /// use this dog this one wont lag - ryder
        /// </summary>
        /// <returns></returns>
        private async Task<bool> InstallYorotFoReal()
        {
            await Task.Run(async () =>
            {
                try
                {
                    if (Directory.Exists(YorotFolderPath))
                    {
                        Directory.Delete(YorotFolderPath, true);
                    }
                    Directory.CreateDirectory(YorotFolderPath);
                    ZipFile.ExtractToDirectory(Settings.WorkFolder + versionToInstall.VersionText + ".htpackage", YorotFolderPath, Encoding.UTF8);
                    installDoneCount++;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    await Task.Run(() => GetBackup(true));
                }
                try
                {
                    appShortcut();
                    Register();
                    Successful();
                }
                catch (Exception ex)
                {
                    Error(ex);
                    await Task.Run(() => GetBackup(true));
                }
            });
            return false;
        }

        private async void Register()
        {
            await Task.Run(() =>
            {
                // Register to Uninstall Programs list.
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall", true))
                {
                    RegistryKey key1 = key.CreateSubKey("Yorot");

                    key1.SetValue("DisplayName", "Yorot", RegistryValueKind.String);
                    key1.SetValue("DisplayVersion", versionToInstall.VersionText, RegistryValueKind.String);
                    key1.SetValue("DisplayIcon", YorotExists ? YorotPath : YorotBetaPath, RegistryValueKind.String);
                    key1.SetValue("Publisher", "haltroy", RegistryValueKind.String);
                    key1.SetValue("UninstallString", Application.ExecutablePath, RegistryValueKind.String);
                    key1.SetValue("ModifyString", Application.ExecutablePath, RegistryValueKind.String);
                    key1.SetValue("InstallLocation", YorotFolderPath, RegistryValueKind.String);
                    key1.SetValue("NoRemove", "1", RegistryValueKind.DWord);
                    key1.SetValue("NoRepair", "1", RegistryValueKind.DWord);
                }

                // FILE ASSOCIATIONS

                // extensions
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@".html\OpenWithProgids"))
                {
                    if (key == null) { Registry.ClassesRoot.CreateSubKey(@".html\OpenWithProgids"); }
                    key.SetValue("YorotHTML", "", RegistryValueKind.String);
                }
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@".htm\OpenWithProgids"))
                {
                    if (key == null) { Registry.ClassesRoot.CreateSubKey(@".htm\OpenWithProgids"); }
                    key.SetValue("YorotHTML", "", RegistryValueKind.String);
                }

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@".pdf\OpenWithProgids"))
                {
                    if (key == null) { Registry.ClassesRoot.CreateSubKey(@".pdf\OpenWithProgids"); }
                    key.SetValue("YorotPDF", "", RegistryValueKind.String);
                }

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@".http\OpenWithProgids"))
                {
                    if (key == null) { Registry.ClassesRoot.CreateSubKey(@".http\OpenWithProgids"); }
                    key.SetValue("YorotHTTP", "", RegistryValueKind.String);
                }
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@".https\OpenWithProgids"))
                {
                    if (key == null) { Registry.ClassesRoot.CreateSubKey(@".https\OpenWithProgids"); }
                    key.SetValue("YorotHTTP", "", RegistryValueKind.String);
                }

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@".kef\OpenWithProgids"))
                {
                    if (key == null) { Registry.ClassesRoot.CreateSubKey(@".kef\OpenWithProgids"); }
                    key.SetValue("YorotEXT", "", RegistryValueKind.String);
                }
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@".ktf\OpenWithProgids"))
                {
                    if (key == null) { Registry.ClassesRoot.CreateSubKey(@".ktf\OpenWithProgids"); }
                    key.SetValue("YorotEXT", "", RegistryValueKind.String);
                }
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@".klf\OpenWithProgids"))
                {
                    if (key == null) { Registry.ClassesRoot.CreateSubKey(@".klf\OpenWithProgids"); }
                    key.SetValue("YorotEXT", "", RegistryValueKind.String);
                }

                // progids

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"YorotEXT"))
                {
                    key.SetValue("", "Yorot Addon", RegistryValueKind.String);
                    key.SetValue("AppUserModelID", "Yorot", RegistryValueKind.String);
                    RegistryKey appKey = key.CreateSubKey("Application");
                    appKey.SetValue("AppUserModelID", "Yorot");
                    appKey.SetValue("ApplicationIcon", YorotExists ? YorotPath : YorotBetaPath);
                    appKey.SetValue("ApplicationName", "Yorot");
                    appKey.SetValue("ApplicationDecription", "Surf the universe.");
                    appKey.SetValue("ApplicationCompany", "haltroy");
                    key.CreateSubKey("DefaultIcon").SetValue("", YorotExists ? YorotPath : YorotBetaPath, RegistryValueKind.String);
                    RegistryKey shellKey = key.CreateSubKey("shell");
                    RegistryKey openKey = shellKey.CreateSubKey("open");
                    openKey.CreateSubKey("command").SetValue("", "\"" + (YorotExists ? YorotPath : YorotBetaPath) + "\" %1");
                }

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"YorotPDF"))
                {
                    key.SetValue("", "Yorot PDF Document", RegistryValueKind.String);
                    key.SetValue("AppUserModelID", "Yorot", RegistryValueKind.String);
                    RegistryKey appKey = key.CreateSubKey("Application");
                    appKey.SetValue("AppUserModelID", "Yorot");
                    appKey.SetValue("ApplicationIcon", YorotExists ? YorotPath : YorotBetaPath);
                    appKey.SetValue("ApplicationName", "Yorot");
                    appKey.SetValue("ApplicationDecription", "Surf the universe.");
                    appKey.SetValue("ApplicationCompany", "haltroy");
                    key.CreateSubKey("DefaultIcon").SetValue("", YorotExists ? YorotPath : YorotBetaPath, RegistryValueKind.String);
                    RegistryKey shellKey = key.CreateSubKey("shell");
                    RegistryKey openKey = shellKey.CreateSubKey("open");
                    openKey.CreateSubKey("command").SetValue("", "\"" + (YorotExists ? YorotPath : YorotBetaPath) + "\" %1");
                }

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"YorotHTML"))
                {
                    key.SetValue("", "Yorot HTML Document", RegistryValueKind.String);
                    key.SetValue("AppUserModelID", "Yorot", RegistryValueKind.String);
                    RegistryKey appKey = key.CreateSubKey("Application");
                    appKey.SetValue("AppUserModelID", "Yorot");
                    appKey.SetValue("ApplicationIcon", YorotExists ? YorotPath : YorotBetaPath);
                    appKey.SetValue("ApplicationName", "Yorot");
                    appKey.SetValue("ApplicationDecription", "Surf the universe.");
                    appKey.SetValue("ApplicationCompany", "haltroy");
                    key.CreateSubKey("DefaultIcon").SetValue("", YorotExists ? YorotPath : YorotBetaPath, RegistryValueKind.String);
                    RegistryKey shellKey = key.CreateSubKey("shell");
                    RegistryKey openKey = shellKey.CreateSubKey("open");
                    openKey.CreateSubKey("command").SetValue("", "\"" + (YorotExists ? YorotPath : YorotBetaPath) + "\" %1");
                }

                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"YorotHTTP"))
                {
                    key.SetValue("", "Yorot Site", RegistryValueKind.String);
                    key.SetValue("AppUserModelID", "Yorot", RegistryValueKind.String);
                    RegistryKey appKey = key.CreateSubKey("Application");
                    appKey.SetValue("AppUserModelID", "Yorot");
                    appKey.SetValue("ApplicationIcon", YorotExists ? YorotPath : YorotBetaPath);
                    appKey.SetValue("ApplicationName", "Yorot");
                    appKey.SetValue("ApplicationDecription", "Surf the universe.");
                    appKey.SetValue("ApplicationCompany", "haltroy");
                    key.CreateSubKey("DefaultIcon").SetValue("", YorotExists ? YorotPath : YorotBetaPath, RegistryValueKind.String);
                    RegistryKey shellKey = key.CreateSubKey("shell");
                    RegistryKey openKey = shellKey.CreateSubKey("open");
                    openKey.CreateSubKey("command").SetValue("", "\"" + (YorotExists ? YorotPath : YorotBetaPath) + "\" %1");
                }

                // PROTOCOL

                if (versionToInstall.Reg == RegType.StandartWithProtocol || versionToInstall.Reg == RegType.StandartWithCommandProtocol)
                {
                    using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"Yorot"))
                    {
                        key.CreateSubKey("DefaultIcon").SetValue("", YorotExists ? YorotPath : YorotBetaPath);
                        RegistryKey shellKey = key.CreateSubKey("shell");
                        RegistryKey openKey = shellKey.CreateSubKey("open");
                        openKey.CreateSubKey("command").SetValue("", "\"" + (YorotExists ? YorotPath : YorotBetaPath) + "\" " + (versionToInstall.Reg == RegType.StandartWithCommandProtocol ? "Yorot://command/?c=" : "") + "%1", RegistryValueKind.String);
                        key.SetValue("", "URL:Yorot protocol", RegistryValueKind.String);
                    }
                }
                installDoneCount++;
            });
        }

        private async void Unregister()
        {
            await Task.Run(() =>
            {
                // Unregister to Uninstall Programs list.
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall", true))
                {
                    key.DeleteSubKeyTree("Yorot", false);
                }

                // Unregister the rest
                using (RegistryKey key = Registry.ClassesRoot)
                {
                    using (RegistryKey subkey = key.CreateSubKey(@".html\OpenWithProgids"))
                    {
                        subkey.DeleteValue("YorotHTML", false);
                    }
                    using (RegistryKey subkey = key.CreateSubKey(@".htm\OpenWithProgids"))
                    {
                        subkey.DeleteValue("YorotHTML", false);
                    }

                    using (RegistryKey subkey = key.CreateSubKey(@".pdf\OpenWithProgids"))
                    {
                        subkey.DeleteValue("YorotPDF", false);
                    }

                    using (RegistryKey subkey = key.CreateSubKey(@".http\OpenWithProgids"))
                    {
                        subkey.DeleteValue("YorotHTTP", false);
                    }
                    using (RegistryKey subkey = key.CreateSubKey(@".https\OpenWithProgids"))
                    {
                        subkey.DeleteValue("YorotHTTP", false);
                    }

                    using (RegistryKey subkey = key.CreateSubKey(@".kef\OpenWithProgids"))
                    {
                        subkey.DeleteValue("YorotEXT", false);
                    }
                    using (RegistryKey subkey = key.CreateSubKey(@".ktf\OpenWithProgids"))
                    {
                        subkey.DeleteValue("YorotEXT", false);
                    }
                    using (RegistryKey subkey = key.CreateSubKey(@".klf\OpenWithProgids"))
                    {
                        subkey.DeleteValue("YorotEXT", false);
                    }

                    key.DeleteSubKeyTree("Yorot", false);
                    key.DeleteSubKeyTree("YorotEXT", false);
                    key.DeleteSubKeyTree("YorotPDF", false);
                    key.DeleteSubKeyTree("YorotHTTP", false);
                    key.DeleteSubKeyTree("YorotHTML", false);
                }
            });
        }

        private void btChangeVer_Click(object sender, EventArgs e)
        {
            lbModifyDesc.Enabled = false;
            btRepair.Enabled = false;
            btUninstall.Enabled = false;
            btChangeVer.Enabled = false;
            pChangeVer.Visible = true;
            pChangeVer.Enabled = true;
            rbPreOut.Text = UIChangeVerO1.Replace("[PREOUT]", VersionManager.GetVersionFromVersionNo(VersionManager.PreOutVerNumber).VersionText);
            rbPerOut_CheckedChanged(this, new EventArgs());
            rbStable_CheckedChanged(this, new EventArgs());
            rbStable.Text = UIChangeVerO2.Replace("[LATEST]", VersionManager.GetVersionFromVersionNo(VersionManager.LatestVersionNumber).VersionText);
            if (supportsLatestPreOut())
            {
                lbPerOutReq.Text = UIPreOutAvailable;
                rbPreOut.Enabled = true;
            }
            else
            {
                lbPerOutReq.Text = UIPreOutDisable;
                rbPreOut.Enabled = false;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            lbModifyDesc.Enabled = true;
            btRepair.Enabled = true;
            btUninstall.Enabled = true;
            btChangeVer.Enabled = true;
            pChangeVer.Visible = false;
            pChangeVer.Enabled = false;
        }

        private void rbPerOut_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPreOut.Checked)
            {
                rbStable.Checked = false;
                rbOld.Checked = false;
                cbOld.Enabled = false;
                lbVersionToInstall.Enabled = false;
                YorotVersion ver = VersionManager.GetVersionFromVersionNo(VersionManager.PreOutVerNumber);
                versionToInstall = ver;
                btInstall1.Text = UIInstallVer.Replace("[VER]", ver.VersionText);
            }
        }

        private void rbStable_CheckedChanged(object sender, EventArgs e)
        {
            if (rbStable.Checked)
            {
                rbPreOut.Checked = false;
                rbOld.Checked = false;
                cbOld.Enabled = false;
                lbVersionToInstall.Enabled = false;
                YorotVersion ver = VersionManager.GetVersionFromVersionNo(VersionManager.LatestVersionNumber);
                versionToInstall = ver;
                btInstall1.Text = UIInstallVer.Replace("[VER]", ver.VersionText);
            }
        }

        private void rbOld_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOld.Checked)
            {
                rbPreOut.Checked = false;
                rbPreOut.Checked = false;
                cbOld.Enabled = true;
                lbVersionToInstall.Enabled = true;
                cbOld.Items.Clear();
                for (int i = VersionManager.PreOutVersions.Count + 1; i < VersionManager.Versions.Count; i++)
                {
                    if (i != VersionManager.LatestVersionNumber && i != VersionManager.PreOutVerNumber)
                    {
                        YorotVersion ver = VersionManager.Versions[i];
                        cbOld.Items.Add(ver.VersionText + " (" + ver.VersionNo + ")");
                    }
                }
                cbOld.SelectedIndex = 0;
            }
        }

        private void cbOld_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indexOfO = cbOld.Text.IndexOf("(") + 1;
            int indexOfC = cbOld.Text.IndexOf(")");
            int getVerNoLenght = indexOfC - indexOfO;
            int verNo = Convert.ToInt32(cbOld.Text.Substring(indexOfO, getVerNoLenght));
            YorotVersion ver = VersionManager.GetVersionFromVersionNo(verNo);
            btInstall1.Text = UIInstallVer.Replace("[VER]", ver.VersionText);
            if (ver.isMissing)
            {
                lbInstallError.Text = UIChangeVerMissing;
                btInstall1.Enabled = false;
                return;
            }
            if (!supportsThisVer(ver))
            {
                lbInstallError.Text = UIChangeVerArchNotSupported;
                btInstall1.Enabled = false;
                return;
            }
            versionToInstall = ver;
            lbInstallError.Text = "";
            btInstall1.Enabled = true;
        }

        private void btRepair_Click(object sender, EventArgs e)
        {
            string YorotPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Haltroy\\Yorot\\Yorot.exe";
            string YorotBetaPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Haltroy\\Yorot\\Yorot Beta.exe";
            bool YorotExists = File.Exists(YorotPath);
            YorotVersion current = VersionManager.GetVersionFromVersionName(FileVersionInfo.GetVersionInfo(YorotExists ? YorotPath : YorotBetaPath).ProductVersion);
            if (current is null) //PreOut
            {
                versionToInstall = VersionManager.GetVersionFromVersionNo(VersionManager.PreOutVerNumber);
            }
            else
            {
                versionToInstall = VersionManager.GetVersionFromVersionNo(VersionManager.LatestVersionNumber);
            }
            if (btRepair.Text == UIUpdateButton) { DoneType = DoneType.Update; } else { DoneType = DoneType.Repair; }
            StartInstallation(isShiftPressed);
        }

        private void btInstall1_Click(object sender, EventArgs e)
        {
            if (rbPreOut.Checked)
            {
                versionToInstall = VersionManager.GetVersionFromVersionNo(VersionManager.PreOutVerNumber);
            }
            else if (rbStable.Checked)
            {
                versionToInstall = VersionManager.GetVersionFromVersionNo(VersionManager.LatestVersionNumber);
            }
            else if (rbOld.Checked)
            {
                int indexOfO = cbOld.Text.IndexOf("(") + 1;
                int indexOfC = cbOld.Text.IndexOf(")");
                int getVerNoLenght = indexOfC - indexOfO;
                int verNo = Convert.ToInt32(cbOld.Text.Substring(indexOfO, getVerNoLenght));
                YorotVersion ver = VersionManager.GetVersionFromVersionNo(verNo);
                versionToInstall = ver;
            }
            DoneType = DoneType.Install;
            StartInstallation();
        }

        private void btInstall_Click(object sender, EventArgs e)
        {
            versionToInstall = VersionManager.GetVersionFromVersionNo(VersionManager.LatestVersionNumber);
            DoneType = DoneType.Install;
            StartInstallation(isShiftPressed);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!allowClose) { e.Cancel = true; }
        }

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            lbDoneDesc.MaximumSize = new Size(tbDoneError.Width, 0);
        }

        private void btSendFeedback_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Haltroy/Yorot/issues/new/choose");
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            if (DoneType == DoneType.Install)
            {
                Process.Start(YorotExists ? YorotPath : YorotBetaPath, "-oobe");
            }
            Settings.Save();
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbInstallCount.Text = (installDoneCount == installJobCount ? installDoneCount : installDoneCount + 1) + "/" + installJobCount;
            if (installDoneCount == 0) { lbInstallInfo.Text = UICreateRecovery; }
            else if (installDoneCount == installJobCount - 2) { lbInstallInfo.Text = UICreateShortcut; }
            else if (installDoneCount == installJobCount - 1) { lbInstallInfo.Text = RegistryStart; }
            else
            {
                lbInstallInfo.Text = workingReq != null ? workingReq.Name : DownloadYorotDesktop;
            }
            double perc = (Convert.ToDouble(installDoneCount) / Convert.ToDouble(installJobCount)) * 100;
            pbInstall.Width = Convert.ToInt32(perc) * (pInstall.Width / 100);
            if (installDoneCount == installJobCount) { Successful(); }
        }

        private void btUninstall_Click(object sender, EventArgs e)
        {
            Directory.Delete(YorotFolderPath, true);
            Unregister();
            DoneType = DoneType.Uninstall;
            Successful();
        }

        private bool isShiftPressed;
        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            isShiftPressed = e.Shift;
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            isShiftPressed = !e.Shift;
        }
    }
    public class StringEventhHybrid
    {
        public string String { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; }
        public StringType Type { get; set; }
        public delegate void EventDelegate(object sender, EventArgs e);
        public event EventDelegate Event;
        public void RunEvent(object sender, EventArgs e)
        {
            if (Event != null)
            {
                Event(sender, e);
            }
        }

        public override string ToString()
        {
            return Type == StringType.String ? " [STRING]" + String : " [FILE]" + String + "-" + String2 + "-" + String3;
        }

        public enum StringType
        {
            String,
            File
        }
    }
    internal enum DoneType
    {
        Install,
        Repair,
        Update,
        Uninstall
    }
    public enum RegType
    {
        Standart,
        StandartWithProtocol,
        StandartWithCommandProtocol,
    }
}
