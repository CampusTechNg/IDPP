using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SourceAFIS.Simple;

namespace IDPP.LocalServer
{
    public class DedupForm : PanelForm
    {
        Button btnScan;
        CheckBox autoDel;
        Panel pnlResult;

        static AfisEngine Afis = new AfisEngine();
        static IdpDb db = new IdpDb();

        public DedupForm(AppWindow owner) : base(owner)
        {
            btnScan = new Button()
            {
                //FlatStyle = FlatStyle.Popup,
                Location = new Point(20, 20),
                Text = "Scan"
            };
            this.Controls.Add(btnScan);
            btnScan.Click += BtnScan_Click;

            autoDel = new CheckBox()
            {
                AutoSize = false,
                Location = new Point(btnScan.Right + 10, btnScan.Top),
                Text = "Automatically delete duplicates",
                Width = 400
            };
            this.Controls.Add(autoDel);

            pnlResult = new Panel()
            {
                AutoScroll = true,
                Location = new Point(btnScan.Location.X, btnScan.Bottom + 10)
            };
            this.Controls.Add(pnlResult);

            this.SizeChanged += delegate 
            {
                pnlResult.Size = new Size(this.Width - 40, this.Height - (50 + btnScan.Height));
            };
        }

        private void BtnScan_Click(object sender, EventArgs e)
        {
            pnlResult.Controls.Clear();
            btnScan.Enabled = false;
            var defCsr = pnlResult.Cursor;
            pnlResult.Cursor = Cursors.WaitCursor;

            List<IEnumerable<Person>> allConflicts = new List<IEnumerable<Person>>();
            var allPersons = db.GetPersons().ToList();
            while (allPersons.Count() > 0)
            {
                var person = allPersons.ToArray()[0];
                var matches = Afis.Identify(person, allPersons);
                if (matches.Count() > 1) allConflicts.Add(matches);
                matches.ToList().ForEach(p => allPersons.Remove((IdpPerson)p));
            }

            int index = 0;
            int locX = 2, locY = 2;
            foreach (var conflict in allConflicts)
            {
                index++;

                Label lbl = new Label()
                {
                    AutoSize = false,
                    Font = new Font(this.Font.FontFamily, 16.0f, FontStyle.Bold),
                    Location = new Point(locX, locY),
                    Text = "Conflict #" + index,
                    Size = new Size(400, 40)
                };
                pnlResult.Controls.Add(lbl);
                locY += lbl.Height + 5;

                int personIndex = 0;
                foreach (var person in conflict)
                {
                    var idp = ((IdpPerson)person);
                    PictureBox picImage = new PictureBox()
                    {
                        Location = new Point(locX, locY),
                        Size = new Size(100, 100)
                    };
                    picImage.Image = idp.Photo;
                    pnlResult.Controls.Add(picImage);

                    Label lblName = new Label()
                    {
                        AutoSize = false,
                        Location = new Point(picImage.Right + 5, picImage.Top),
                        Text = idp.FirstName + (!string.IsNullOrWhiteSpace(idp.LastName) ? " " + idp.LastName : "") +
                            (!string.IsNullOrWhiteSpace(idp.OtherNames) ? " " + idp.OtherNames : ""),
                        Width = 400,
                    };
                    pnlResult.Controls.Add(lblName);

                    Button btnDelete = new Button()
                    {
                        FlatStyle = FlatStyle.Popup,
                        Location = new Point(lblName.Left, lblName.Bottom + 5),
                        Text = "Delete...",

                        Tag = idp.ID,
                    };
                    pnlResult.Controls.Add(btnDelete);
                    btnDelete.Click += delegate (object ss, EventArgs ee)
                    {
                        Button btn = (Button)ss;
                        if (db.DeletePerson(btn.Tag.ToString()))
                        {
                            btn.Text = "Deleted " + Convert.ToChar(10004).ToString();
                            btn.Enabled = false;
                        }
                    };

                    if (autoDel.Checked && personIndex > 0) btnDelete.PerformClick();

                    locY += picImage.Height + 5;
                    personIndex++;
                }

                Label lblLine = new Label()
                {
                    BackColor = Color.Black,
                    Location = new Point(locX, locY),
                    Size = new Size(pnlResult.Width - 4, 2)
                };
                pnlResult.Controls.Add(lblLine);
            }

            btnScan.Enabled = true;
            pnlResult.Cursor = defCsr;
        }
    }
}
