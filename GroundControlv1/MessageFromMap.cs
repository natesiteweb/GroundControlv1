using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroundControlv1
{
    public class MessageFromMap
    {
        public Form1 form1;

        public void setCoords(float marker_count, float marker_index, float lat, float lon, float altitude)
        {
            //form1.AccessStatusBuffer(marker_index.ToString() + ":" + lat.ToString() + ":" + lon.ToString() + "\n");

            form1.markerAltitudes[(int)marker_index + 4] = altitude;
            form1.markerPoints[(int)marker_index + 4].Y = (int)(lat * 10000000);
            form1.markerPoints[(int)marker_index + 4].X = (int)(lon * 10000000);

            form1.total_marker_count = (int)marker_count;
        }

        public float setFirstAlt()
        {
            float getAlt = 0f;

            AltitudeSetForm popup = new AltitudeSetForm();
            DialogResult dialogresult = popup.ShowDialog();

            getAlt = popup.GetAltitude();

            popup.Dispose();

            return getAlt;
        }
    }
}
