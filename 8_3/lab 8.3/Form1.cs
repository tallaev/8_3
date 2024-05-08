using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace lab_8._3
{
    public partial class Form1 : Form
    {
        private readonly Random random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int numTrials = int.Parse(num.Text);
            double[] probabilities = CalculateProbabilities();

            List<int> events = GenerateEvents(numTrials, probabilities);
            DisplayEmpiricalDistribution(events);
        }

        private double[] CalculateProbabilities()
        {
            double[] probabilities = { double.Parse(Prob1.Text), double.Parse(Prob2.Text), double.Parse(Prob3.Text), double.Parse(Prob4.Text) };
            double lastProbability = 1 - probabilities.Sum();
            Prob5.Text = lastProbability.ToString();
            return probabilities.Concat(new[] { lastProbability }).ToArray();
        }

        private void DisplayEmpiricalDistribution(List<int> events)
        {
            var eventCounts = events.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
            PlotChart(eventCounts);
        }

        private List<int> GenerateEvents(int numTrials, double[] probabilities)
        {
            var events = new List<int>();

            for (int i = 0; i < numTrials; i++)
                events.Add(GenerateEvent(probabilities));

            return events;
        }

        private int GenerateEvent(double[] probabilities)
        {
            double rnd = random.NextDouble();
            double cumulativeProbability = 0;

            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulativeProbability += probabilities[i];
                if (rnd < cumulativeProbability)
                    return i + 1;
            }

            return probabilities.Length;
        }

        private void PlotChart(Dictionary<int, int> eventCounts)
        {
            chart.Series.Clear();
            Series series = new Series("Event Frequency") { ChartType = SeriesChartType.Column };

            foreach (var kvp in eventCounts.OrderBy(x => x.Key))
                series.Points.AddXY(kvp.Key, kvp.Value);

            chart.Series.Add(series);
            chart.ChartAreas[0].AxisX.Interval = 1;
        }
    }
}