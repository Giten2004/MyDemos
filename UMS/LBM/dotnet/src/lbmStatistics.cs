/*
  Copyright (c) 2005-2013 Informatica Corporation  Permission is granted to licensees to use
  or alter this software for any purpose, including commercial applications,
  according to the terms laid out in the Software License Agreement.

  This source code example is provided by Informatica for educational
  and evaluation purposes only.

  THE SOFTWARE IS PROVIDED "AS IS" AND INFORMATICA DISCLAIMS ALL WARRANTIES 
  EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION, ANY IMPLIED WARRANTIES OF 
  NON-INFRINGEMENT, MERCHANTABILITY OR FITNESS FOR A PARTICULAR 
  PURPOSE.  INFORMATICA DOES NOT WARRANT THAT USE OF THE SOFTWARE WILL BE 
  UNINTERRUPTED OR ERROR-FREE.  INFORMATICA SHALL NOT, UNDER ANY CIRCUMSTANCES, BE 
  LIABLE TO LICENSEE FOR LOST PROFITS, CONSEQUENTIAL, INCIDENTAL, SPECIAL OR 
  INDIRECT DAMAGES ARISING OUT OF OR RELATED TO THIS AGREEMENT OR THE 
  TRANSACTIONS CONTEMPLATED HEREUNDER, EVEN IF INFORMATICA HAS BEEN APPRISED OF 
  THE LIKELIHOOD OF SUCH DAMAGES.
*/
using System;

namespace LBMApplication
{
    /*
     *  lbmStatistics class
     *
     *  Original Port Date: March 2013
     *              Author: hwong
     */
    class lbmStatistics
    {
    /* 
        This class has been ported from a C library, and hence,
        has code that follows a different copyright. That copyright
        message is included here...


        Copyright (c) 2011 Anil Madhavapeddy <anil@recoil.org>

        Permission is hereby granted, free of charge, to any person
        obtaining a copy of this software and associated documentation
        files (the "Software"), to deal in the Software without
        restriction, including without limitation the rights to use,
        copy, modify, merge, publish, distribute, sublicense, and/or sell
        copies of the Software, and to permit persons to whom the
        Software is furnished to do so, subject to the following
        conditions:

        The above copyright notice and this permission notice shall be
        included in all copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
        EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
        OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
        NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
        HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
        WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
        FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
        OTHER DEALINGS IN THE SOFTWARE.
    */
        public double [] data;

        public double mean;
        public double sample_sd;
        public double sample_skew;
        public double sample_kurtosis;

        public double alpha;
        public double beta;

        public lbmStatistics calcSummaryStats(double [] data) {
            /* On-line calculation of mean, variance, skew and kurtosis
               lifted straight from wikipedia. */
            double mean = 0;
            double m2 = 0;
            double m3 = 0;
            double m4 = 0;
            double delta;
            double delta_n;
            double variance;
            double sd;
            double skew;
            double kurtosis;
            double n;
            int i;

            for (i = 0; i < data.Length; i++) {
                n = i + 1;
                delta = data[i] - mean;
                delta_n = delta / n;
                mean = (mean * i) / n + data[i]/n;
                m4 = m4 + delta_n * delta_n * delta_n * delta * (n - 1) * (n * n - 3 * n + 3) + 6 * delta_n * delta_n * m2 - 4 * delta_n * m3;
                m3 = m3 + delta_n * delta_n * delta * (n - 1) * (n - 2) - 3 * delta_n * m2;
                m2 = m2 + delta_n * delta * (n - 1);
            }

            variance = m2 / data.Length;
            sd = Math.Sqrt(variance);
            skew = m3/(data.Length * sd * sd * sd);
            kurtosis = data.Length * m4 / (m2*m2) - 3;

            this.mean = mean;
            this.sample_sd = sd;
            this.sample_skew = skew;
            this.sample_kurtosis = kurtosis;

            this.data = data;
            return this;
        }

        public double pointToPercentile(double point) {
            int low, high;
            int probe;

            if (point < data[0])
                return 0;
            else if (point > data[data.Length-1])
                return 100;
            low = 0;
            high = data.Length;
            while (low + 1 < high) {
                /* Invariant: everything in slots before @low is less than @point,
                   everything in slots at or after @high is greater than
                   @point. */
                probe = (high + low) / 2;
                if (point > data[probe]) {
                    low = probe + 1;
                } else if (point < data[probe]) {
                    high = probe;
                } else {
                    /* The probe is now in the range of data which is equal to
                       point. */
                    return doProbeIsPointPercentile(point, low, high, probe);
                }
            }
            if (high == low + 1) {
                if (point < data[low]) {
                    low--;
                    high--;
                }
                if (data[low] == point) {
                    probe = low;
                    return doProbeIsPointPercentile(point, low, high, probe);
                } else if (data[high] == point) {
                    probe = high;
                    return doProbeIsPointPercentile(point, low, high, probe);
                } else {
                    return doLinearInterpolatePercentile(point, low, high);
                }
            } else {
                if (low == 0) {
                    return 0;
                } else {
                    low = high - 1;
                    return doLinearInterpolatePercentile(point, low, high);
                }
            }
        }

        // Argh. The orig code has gotos, so I had to break it out to methods (see pointToPercentile)
        private double doProbeIsPointPercentile(double point, int low, int high, int probe) {
            low = probe;
            while (low >= 0 && data[low] == point)
                low--;
            high = probe;
            while (high < data.Length && data[high] == point)
                high++;
            return (high + low) * 50.0 / data.Length;
        }
    
        // Argh. The orig code has gotos, so I had to break it out to methods (see pointToPercentile)
        private double doLinearInterpolatePercentile(double point, int low, int high) {
            double y1, y2, num, denum;

            y1 = data[low];
            y2 = data[high];
            num = (point + y2 * low - high * y1) * 100.0 / data.Length;
            denum = y2 - y1;
            if (Math.Abs(denum / num) < 0.01) {
                /* The two points we're trying to interpolate between are so close
                   together that we risk numerical error, so we can't use the
                   normal formula.  Fortunately, if they're that close together
                   then it doesn't really matter, and we can use a simple
                   average. */
                return (low + high) * 50.0 / data.Length;
            } else {
                return num / denum;
            }
        }

        public void printSummaryStats(System.IO.TextWriter f) {
            double [] sd_percentiles = new double[7];
            int i;

            string output = String.Format("\tMean {0:0.00}, sample sd {1:0.00}, sample skew {2:0.00}, sample kurtosis {3:0.00}",
                              mean, sample_sd, sample_skew, sample_kurtosis);
            f.WriteLine(output);

            output = String.Format("\tQuintiles: {0:0.00}, {1:0.00}, {2:0.00}, {3:0.00}, {4:0.00}, {5:0.00}",
                   data[0],
                   data[data.Length / 5],
                   data[data.Length * 2 / 5],
                   data[data.Length * 3 / 5],
                   data[data.Length * 4 / 5],
                   data[data.Length - 1]);
            f.WriteLine(output);

            output = String.Format("\t5% {0:0.00}, median {1:0.00}, 95% {2:0.00}",
                   data[data.Length / 20],
                   data[data.Length / 2],
                   data[data.Length * 19 / 20]);
            f.WriteLine(output);

            /* Also look at how deltas from the mean, in multiples of the SD,
               map onto percentiles, to get more hints about non-normality. */
            for (i = 0; i < 7; i++) {
                double point = mean + sample_sd * (i - 3);
                sd_percentiles[i] = pointToPercentile(point);
            }
            output = String.Format("\tSD percentiles: -3 -> {0:0.00}%, -2 -> {1:0.00}%, -1 -> {2:0.00}%, 0 -> {3:0.00}%, 1 -> {4:0.00}%, 2 -> {5:0.00}%, 3 -> {6:0.00}%",
                sd_percentiles[0],
                sd_percentiles[1],
                sd_percentiles[2],
                sd_percentiles[3],
                sd_percentiles[4],
                sd_percentiles[5],
                sd_percentiles[6]);
            f.WriteLine(output);
        }

        public lbmStatistics linearRegression(double [] data) {
            double x_bar;
            double x_bar2;
            double x_y_bar;
            double y_bar;
            int i;

            x_y_bar = 0;
            y_bar = 0;

            for (i = 0; i < data.Length; i++) {
                x_y_bar += data[i] * (i + 1);
                y_bar += data[i];
            }

            x_y_bar /= data.Length;
            y_bar /= data.Length;

            x_bar = data.Length / 2.0 + 1;
            x_bar2 = (data.Length + 2.0) * (2.0 * data.Length + 1) / 6.0;

            beta = (x_y_bar - x_bar * y_bar) / (x_bar2 - x_bar * x_bar);
            alpha = y_bar - beta * x_bar;

            /* Norm so that xs run from 0 to 1, rather than from 0 to
               sample size, because that's a bit easier to think about. */
            beta *= data.Length;
            return this;
        }

        /* Note: didn't port "summarise_samples" -- see c code if needed
         */
    }

    /*
     *  The latency calculator that I "borrowed" from lbmpong. 
     *  This code is no longer is use, as it has been replaced with the lbmStatistics class, but it
     *  is kept here -- just-in-case.
     * 
     *  Creation Date: March 2013
     *         Author: hwong
     */
    class lbmponglatencycalculator
    {
        private double[] timestamps;
        private double total;
        private double average;
        private double medium;
        private double minimum = 1000000000;
        private double maximum = -1000000000;
        private double deviation;
        private bool doMediumStdDev;

        // Note: Medium and StdDev requires a sorted data set, and currently, it is incredibly slow
        //       (turn off for now)
        public lbmponglatencycalculator(double[] ts, bool calcMediumStdDev)
        {
            timestamps = new double[ts.Length];
            Array.Copy(ts, timestamps, ts.Length);
            doMediumStdDev = calcMediumStdDev;

            if (doMediumStdDev) sortTimestamps();
            calcDetails();
        }

        private void sortTimestamps()
        {
            int r;
            bool changed;
            double t;
            long msgs = timestamps.Length;

            /* bubble sort the result set (this is slow !!) */
            do
            {
                changed = false;

                for (r = 0; r < msgs - 1; r++)
                {
                    if (timestamps[r] > timestamps[r + 1])
                    {
                        t = timestamps[r];
                        timestamps[r] = timestamps[r + 1];
                        timestamps[r + 1] = t;
                        changed = true;
                    }
                }
                msgs--;
            }
            while (changed);
        }

        private void calcDetails()
        {
            int msgs = timestamps.Length;
            total = 0.0;
            for (int r = 0; r < msgs; r++)
            {
                if (minimum > timestamps[r]) minimum = timestamps[r];
                if (maximum < timestamps[r]) maximum = timestamps[r];
                total += timestamps[r];
            }
            average = total / msgs;

            if (doMediumStdDev)
            {
                if ((msgs & 1) == 1)
                {
                    /* Odd number of data elements - take middle */
                    medium = timestamps[(int)(msgs / 2) + 1];
                }
                else
                {
                    /* Even number of data element avg the two middle ones */
                    medium = (timestamps[(int)(msgs / 2)] + timestamps[((int)msgs / 2) + 1]) / 2;
                }

                /* Subtract the mean from the data points, square them and sum them */
                double sum = 0.0;
                for (int r = 0; r < timestamps.Length; r++)
                {
                    timestamps[r] -= average;
                    timestamps[r] *= timestamps[r];
                    sum += timestamps[r];
                }
                sum /= (timestamps.Length - 1);
                deviation = System.Math.Sqrt(sum);
            }
        }

        public double getMinimum()
        {
            return minimum;
        }

        public double getMaximum()
        {
            return maximum;
        }

        public double getTotal()
        {
            return total;
        }

        public double getMean()
        {
            return average;
        }

        public double getMedium()
        {
            return medium;
        }

        public double getStddev()
        {
            return deviation;
        }

        public void print_latency(System.IO.TextWriter fstr)
        {
            double avg = getMean();
            double min = getMinimum(), max = getMaximum();
            double latency = getMean() / 2.0;

            if (doMediumStdDev)
            {
                fstr.WriteLine("min/max msg = " + getMinimum().ToString("0.0000") + "/"
                            + getMaximum().ToString("0.0000")
                            + " median/stddev " + getMedium().ToString("0.0000") + "/"
                            + getMedium().ToString("0.0000") + " nsec");
            }
            if (latency > 1000.0)
            {
                fstr.WriteLine("Elapsed time " + (getTotal() / 1000.0) + " usecs "
                              + timestamps.Length + " messages (RTTs). "
                              + "min/avg/max " + (getMinimum() / 1000.0).ToString("0.0000") + "/"
                              + (getMean() / 1000.0).ToString("0.0000") + "/"
                              + (getMaximum() / 1000.0).ToString("0.0000") + " usec RTT\n"
                              + "        " + (latency / 1000.0).ToString("0.0000") + " usec latency");
            }
            else
            {
                fstr.WriteLine("Elapsed time " + getTotal() + " nsecs "
                              + timestamps.Length + " messages (RTTs). "
                              + "min/avg/max " + getMinimum().ToString("0.0000") + "/"
                              + getMean().ToString("0.0000") + "/"
                              + getMaximum().ToString("0.0000") + " nsec RTT\n"
                              + "        " + (latency).ToString("0.0000") + " nsec latency");
            }
            fstr.Flush();
        }
    }
}
