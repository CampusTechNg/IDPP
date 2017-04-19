using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThoughtWorks.QRCode.Codec;
using System.Drawing.Printing;
using ThoughtWorks.QRCode.Codec.Data;

namespace IDPP.LocalServer
{
    public class CodeGenForm : PanelForm
    {
        TabControl tabEncodeQR, tabDecodeQR;
        TabPage pgEncodeQR, pgDecodeQR;

        public CodeGenForm(AppWindow owner) : base(owner)
        {
            this.BackColor = SystemColors.Control;

            pgEncodeQR = new EncodeQRCodePage(owner);
            pgDecodeQR = new DecodeQRCodePage();

            tabEncodeQR = new TabControl();
            tabEncodeQR.Controls.Add(pgEncodeQR);
            this.Controls.Add(tabEncodeQR);

            tabDecodeQR = new TabControl();
            tabDecodeQR.Controls.Add(pgDecodeQR);
            this.Controls.Add(tabDecodeQR);

            this.SizeChanged += delegate 
            {
                tabEncodeQR.Height = tabDecodeQR.Height = this.Height - 20;
                tabEncodeQR.Width = tabDecodeQR.Width = (this.Width / 2) - 20;
                tabEncodeQR.Location = new Point(10, 10);
                tabDecodeQR.Location = new Point(tabEncodeQR.Right + 20, tabEncodeQR.Top);
            };
        }
    }

    public class EncodeQRCodePage : TabPage
    {
        PictureBox picEncode;
        TextBox txtData;
        ComboBox corrLvlList, encodingList, versionList, sizeList;
        Button btnEncode, btnSave, btnPrint;

        AppWindow owner;

        PrintDialog printDialog;
        PrintDocument printDocument;
        SaveFileDialog saveDialog;

        public EncodeQRCodePage(AppWindow owner)
        {
            this.owner = owner;

            printDialog = new PrintDialog()
            {
                UseEXDialog = true,
            };
            printDocument = new PrintDocument() { };
            printDocument.PrintPage += PrintDocument_PrintPage;

            saveDialog = new SaveFileDialog()
            {
                FileName = string.Empty,
                Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG Image|*.png",
                Title = "Save",
            };

            BackColor = Color.White;
            Text = "Encode";

            picEncode = new PictureBox()
            {
                Size = new Size(400, 400)
            };
            this.Controls.Add(picEncode);

            Label lblData = new Label()
            {
                AutoSize = false,
                Text = "Data",
                Width = 50,
            };
            this.Controls.Add(lblData);

            txtData = new TextBox()
            {
            };
            this.Controls.Add(txtData);

            Label lblSize = new Label()
            {
                Text = "Size",
                Width = lblData.Width
            };
            this.Controls.Add(lblSize);

            sizeList = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            for (int size = 1; size <= 8; size++)
            {
                sizeList.Items.Add(size);
            }
            this.Controls.Add(sizeList);
            sizeList.SelectedIndex = 7;

            GroupBox grp = new GroupBox()
            {   
                Height = 110,
                Text = "Advanced"
            };
            this.Controls.Add(grp);

            Label lblCorrLvl = new Label()
            {
                Location = new Point(10, 20),
                Text = "Correction Level",
                Width = 100
            };
            grp.Controls.Add(lblCorrLvl);

            corrLvlList = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(lblCorrLvl.Right, lblCorrLvl.Top),
            };
            corrLvlList.Items.Add("H");
            corrLvlList.Items.Add("L");
            corrLvlList.Items.Add("M");
            corrLvlList.Items.Add("Q");
            grp.Controls.Add(corrLvlList);
            corrLvlList.SelectedIndex = 2;

            Label lblEncoding = new Label()
            {
                Location = new Point(lblCorrLvl.Left, lblCorrLvl.Bottom + 5),
                Text = "Encoding",
                Width = lblCorrLvl.Width
            };
            grp.Controls.Add(lblEncoding);

            encodingList = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(lblEncoding.Right, lblEncoding.Top),
            };
            encodingList.Items.Add("AlphaNumeric");
            encodingList.Items.Add("Byte");
            encodingList.Items.Add("Numeric");
            grp.Controls.Add(encodingList);
            encodingList.SelectedIndex = 1;

            Label lblVersion = new Label()
            {
                Location = new Point(lblEncoding.Left, lblEncoding.Bottom + 5),
                Text = "Version",
                Width = lblCorrLvl.Width
            };
            grp.Controls.Add(lblVersion);

            versionList = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(lblVersion.Right, lblVersion.Top),
            };
            for (int version = 1; version <= 40; version++)
            {
                versionList.Items.Add(version);
            }
            grp.Controls.Add(versionList);
            versionList.SelectedIndex = 6;

            btnEncode = new Button()
            {
                Text = "Encode"
            };
            this.Controls.Add(btnEncode);
            btnEncode.Click += BtnEncode_Click;

            btnSave = new Button()
            {
                Text = "Save"
            };
            this.Controls.Add(btnSave);
            btnSave.Click += BtnSave_Click;

            btnPrint = new Button()
            {
                Text = "Print"
            };
            this.Controls.Add(btnPrint);
            btnPrint.Click += BtnPrint_Click;

            this.SizeChanged += delegate 
            {
                picEncode.Location = new Point((this.Width / 2) - picEncode.Width / 2, 20);
                lblData.Location = new Point(picEncode.Left, picEncode.Bottom + 10);
                txtData.Location = new Point(lblData.Right, lblData.Top);
                txtData.Width = picEncode.Right - txtData.Left;
                lblSize.Location = new Point(lblData.Left, lblData.Bottom + 5);
                sizeList.Location = new Point(lblSize.Right, lblSize.Top);
                sizeList.Width = txtData.Width;
                grp.Location = new Point(picEncode.Left, lblSize.Bottom + 10);
                grp.Width = picEncode.Width;
                corrLvlList.Width = grp.Width - (encodingList.Left + 10);
                encodingList.Width = corrLvlList.Width;
                versionList.Width = corrLvlList.Width;
                btnEncode.Location = new Point(picEncode.Left, grp.Bottom + 10);
                btnSave.Location = new Point(((picEncode.Width - btnSave.Width) / 2) + picEncode.Left, btnEncode.Top);
                btnPrint.Location = new Point(picEncode.Right - btnPrint.Width, btnEncode.Top);
            };
        }

        private void BtnEncode_Click(object sender, EventArgs e)
        {
            if (txtData.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Data must not be empty.");
                return;
            }

            #region Zen.Barcode
            //Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            //picEncode.Image = barcode.Draw(txtData.Text, 100);

            //Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            //picEncode.Image = qrcode.Draw(txtData.Text, 100);
            #endregion

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            String encoding = encodingList.SelectedItem.ToString();
            if (encoding == "Byte")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            }
            else if (encoding == "AlphaNumeric")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
            }
            else if (encoding == "Numeric")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
            }
            try
            {
                int scale = Convert.ToInt16(sizeList.SelectedItem.ToString());
                qrCodeEncoder.QRCodeScale = scale;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid size!");
                return;
            }
            try
            {
                int version = Convert.ToInt16(versionList.SelectedItem.ToString());
                qrCodeEncoder.QRCodeVersion = version;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid version !");
            }

            string errorCorrect = corrLvlList.SelectedItem.ToString();
            if (errorCorrect == "L")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            else if (errorCorrect == "M")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            else if (errorCorrect == "Q")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
            else if (errorCorrect == "H")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;

            Image image;
            String data = txtData.Text;
            qrCodeEncoder.QRCodeForegroundColor = owner.DarkColor;
            image = qrCodeEncoder.Encode(data);

            Font dataFont = new Font(this.Font.FontFamily, 12.0f, FontStyle.Regular);
            SizeF sf = this.CreateGraphics().MeasureString(data, dataFont);
            Bitmap bmp = new Bitmap(image,
                new Size(Math.Max(image.Width, (int)Math.Ceiling(sf.Width)), image.Height + (int)Math.Ceiling(sf.Height)));
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.DrawImage(image, new PointF((bmp.Width - image.Width) / 2, 0));
            g.DrawString(data, dataFont, new SolidBrush(owner.DarkColor),
                new PointF((bmp.Width - sf.Width) / 2, image.Height));
            picEncode.Image = bmp;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveDialog.FileName != "")
                {
                    System.IO.FileStream fs =
                       (System.IO.FileStream)saveDialog.OpenFile();
                    // Saves the Image in the appropriate ImageFormat based upon the
                    // File type selected in the dialog box.
                    // NOTE that the FilterIndex property is one-based.
                    switch (saveDialog.FilterIndex)
                    {
                        case 1:
                            this.picEncode.Image.Save(fs,
                               System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;

                        case 2:
                            this.picEncode.Image.Save(fs,
                               System.Drawing.Imaging.ImageFormat.Bmp);
                            break;

                        case 3:
                            this.picEncode.Image.Save(fs,
                               System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                        case 4:
                            this.picEncode.Image.Save(fs,
                               System.Drawing.Imaging.ImageFormat.Png);
                            break;
                    }

                    MessageBox.Show("Saved successfully!");
                    fs.Close();
                }
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            printDialog.Document = printDocument;
            DialogResult r = printDialog.ShowDialog();
            if (r == DialogResult.OK)
            {
                printDocument.Print();
            }
        }
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(picEncode.Image, 20, 20);
        }
    }

    public class DecodeQRCodePage : TabPage
    {
        PictureBox picDecode;
        TextBox txtData;
        Button btnOpen, btnDecode;

        OpenFileDialog openDialog;

        public DecodeQRCodePage()
        {
            BackColor = Color.White;
            Text = "Decode";

            openDialog = new OpenFileDialog()
            {
                FileName = string.Empty,
                Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG Image|*.png|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            picDecode = new PictureBox()
            {
                Size = new Size(400, 400)
            };
            this.Controls.Add(picDecode);

            Label lblData = new Label()
            {
                AutoSize = false,
                Text = "Data",
                Width = 50,
            };
            this.Controls.Add(lblData);

            txtData = new TextBox()
            {
            };
            this.Controls.Add(txtData);

            btnOpen = new Button()
            {
                Text = "Open"
            };
            this.Controls.Add(btnOpen);
            btnOpen.Click += BtnOpen_Click;

            btnDecode = new Button()
            {
                Text = "Decode"
            };
            this.Controls.Add(btnDecode);
            btnDecode.Click += BtnDecode_Click;

            this.SizeChanged += delegate
            {
                picDecode.Location = new Point((this.Width / 2) - picDecode.Width / 2, 20);
                lblData.Location = new Point(picDecode.Left, picDecode.Bottom + 10);
                txtData.Location = new Point(lblData.Right, lblData.Top);
                txtData.Width = picDecode.Right - txtData.Left;
                btnOpen.Location = new Point((picDecode.Width / 2) - (btnOpen.Width + 10) + picDecode.Left, txtData.Bottom + 10);
                btnDecode.Location = new Point(btnOpen.Right + 20, btnOpen.Top);
            };
        }

        private void BtnDecode_Click(object sender, EventArgs e)
        {
            try
            {
                QRCodeDecoder decoder = new QRCodeDecoder();
                //QRCodeDecoder.Canvas = new ConsoleCanvas();
                String decodedString = decoder.decode(new QRCodeBitmapImage(new Bitmap(picDecode.Image)));
                txtData.Text = decodedString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                String fileName = openDialog.FileName;
                picDecode.Image = new Bitmap(fileName);

            }
        }
    }
}
