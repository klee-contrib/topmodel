import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Tooltip,
  Legend,
  type ChartOptions,
  type ChartData,
} from "chart.js";
import { Line } from "react-chartjs-2";
import type { Series } from "../data/benchmark";

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Tooltip, Legend);

interface Props {
  title: string;
  yLabel: string;
  yMax: number;
  yStep: number;
  series: readonly Series[];
  unit?: string;
}

export default function BenchmarkChart({ title, yLabel, yMax, yStep, series, unit = "" }: Props) {
  const data: ChartData<"line"> = {
    datasets: series.map((s) => ({
      label: s.label,
      data: s.points.map(([x, y]) => ({ x, y })),
      borderColor: s.color,
      backgroundColor: s.color,
      pointRadius: 3,
      pointHoverRadius: 5,
      borderWidth: 2.5,
      tension: 0.25,
      fill: false,
    })),
  };

  const options: ChartOptions<"line"> = {
    responsive: true,
    maintainAspectRatio: false,
    interaction: { mode: "nearest", intersect: false },
    plugins: {
      legend: { position: "right", labels: { boxWidth: 14, boxHeight: 4, font: { size: 12 } } },
      title: { display: true, text: title, font: { size: 16, weight: "600" }, color: "#222" },
      tooltip: {
        callbacks: {
          title: (items) => `${items[0].parsed.x} min`,
          label: (item) => ` ${item.dataset.label} : ${item.parsed.y}${unit}`,
        },
      },
    },
    scales: {
      x: {
        type: "linear",
        title: { display: true, text: "minutes depuis le début du run" },
        min: 0,
        max: 30,
        ticks: { stepSize: 5 },
      },
      y: {
        min: 0,
        max: yMax,
        title: { display: true, text: yLabel },
        ticks: { stepSize: yStep },
      },
    },
  };

  return (
    <div style={{ position: "relative", height: 380, margin: "1rem 0" }}>
      <Line data={data} options={options} />
    </div>
  );
}
