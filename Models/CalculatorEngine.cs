using System;

namespace EngineeringCalculator
{
    public class CalculatorEngine
    {
        // Результаты расчетов
        public double Result1 { get; private set; }
        public double Result2 { get; private set; }
        public double Result3 { get; private set; }
        public double Width0 { get; private set; }
        public double StZapKalib { get; private set; }
        public double Rscrug { get; private set; }
        public double KoefVit { get; private set; }
        public double MarkSt { get; private set; }
        public double Temp { get; private set; }

        // Метод для выполнения расчетов
        public void Calculate(string mode, double width0, double stZapKalib, double rscrug, double koefVit, double temp, double nachDVal = 0)
        {
            switch (mode)
            {
                case "Квадрат-Овал":
                    // Расчеты для режима "Квадрат-Овал"
                    Result1 = width0 + stZapKalib + rscrug;
                    Result2 = koefVit * temp;
                    Result3 = width0 / rscrug;
                    break;

                case "Шестиугольник-Квадрат":
                    // Расчеты для режима "Шестиугольник-Квадрат"
                    Result1 = rscrug * koefVit + width0;
                    Result2 = stZapKalib - temp;
                    Result3 = width0 * koefVit / rscrug;
                    break;

                case "Квадрат-Ромб":
                    // Расчеты для режима "Квадрат-Ромб"
                    Result1 = width0 * temp - koefVit;
                    Result2 = stZapKalib + rscrug;
                    Result3 = temp / koefVit;
                    break;

                default:
                    throw new ArgumentException("Выбран недопустимый режим работы.");
            }
        }

        // Методы для каждого режима работы
        private void CalculateSquareRhombusMode(double Width0, double StZapKalib, double Rscrug, double KoefVit, double Temp)
        {
            Result1 = Width0 + StZapKalib + Rscrug;
            Result2 = KoefVit * Temp;
            Result3 = Width0 / Rscrug;
        }

        private void CalculateSquareOvalMode(double Width0, double StZapKalib, double Rscrug, double KoefVit, double Temp)
        {
            Result1 = Width0 * Temp - KoefVit;
            Result2 = StZapKalib + Rscrug;
            Result3 = Temp / KoefVit;
        }

        private void CalculateHexagonSquareMode(double Width0, double StZapKalib, double Rscrug, double KoefVit, double Temp)
        {
            Result1 = Rscrug * KoefVit + Width0;
            Result2 = StZapKalib - Temp;
            Result3 = Width0 * KoefVit / Rscrug;
        }
    }
}