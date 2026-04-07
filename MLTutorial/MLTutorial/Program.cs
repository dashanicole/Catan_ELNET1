using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;

namespace ProgFitML
{
    public class ProgFitData
    {
        [LoadColumn(0)] public string Gender { get; set; }
        [LoadColumn(1)] public string PrimaryInterest { get; set; }
        [LoadColumn(2)] public string Hobby { get; set; }
        [LoadColumn(3)] public string IncomeLevel { get; set; }
        [LoadColumn(4)] public string NearSchool { get; set; }
        [LoadColumn(5)] public string WorkingStudent { get; set; }
        [LoadColumn(6)] public string InternetAccess { get; set; }
        [LoadColumn(7)] public string MotherEducation { get; set; }
        [LoadColumn(8)] public string FatherEducation { get; set; }
        [LoadColumn(9)] public string TrackStrand { get; set; }
        [LoadColumn(10)] public string GwaRange { get; set; }
        [LoadColumn(11)] public string StudyHours { get; set; }
        [LoadColumn(12)] public string SchoolType { get; set; }
        [LoadColumn(13)] public string Extracurricular { get; set; }
        [LoadColumn(14)] public string FavoriteSubject { get; set; }
        [LoadColumn(15)] public string AcademicAwards { get; set; }
        [LoadColumn(16)] public float Openness1 { get; set; }
        [LoadColumn(17)] public float Openness2 { get; set; }
        [LoadColumn(18)] public float Conscientiousness1 { get; set; }
        [LoadColumn(19)] public float Conscientiousness2 { get; set; }
        [LoadColumn(20)] public float SelfEfficacy1 { get; set; }
        [LoadColumn(21)] public float SelfEfficacy2 { get; set; }
        [LoadColumn(22)] public string ChosenMajor { get; set; }
    }

    public class ProgFitPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedMajor { get; set; }

        public float[] Score { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var mlContext = new MLContext(seed: 1);

            string dataPath = @"C:\Users\DIETHER CATAN\source\repos\MLTutorial\MLTutorial\progfit-data.txt";

            IDataView trainingData = mlContext.Data.LoadFromTextFile<ProgFitData>(
                path: dataPath,
                hasHeader: true,
                separatorChar: '\t');

            var pipeline =
                mlContext.Transforms.Conversion.MapValueToKey(
                    outputColumnName: "Label",
                    inputColumnName: nameof(ProgFitData.ChosenMajor))

                .Append(mlContext.Transforms.Categorical.OneHotEncoding(
                    new[]
                    {
                        new InputOutputColumnPair("GenderEncoded", nameof(ProgFitData.Gender)),
                        new InputOutputColumnPair("InterestEncoded", nameof(ProgFitData.PrimaryInterest)),
                        new InputOutputColumnPair("HobbyEncoded", nameof(ProgFitData.Hobby)),
                        new InputOutputColumnPair("IncomeEncoded", nameof(ProgFitData.IncomeLevel)),
                        new InputOutputColumnPair("NearSchoolEncoded", nameof(ProgFitData.NearSchool)),
                        new InputOutputColumnPair("WorkingEncoded", nameof(ProgFitData.WorkingStudent)),
                        new InputOutputColumnPair("InternetEncoded", nameof(ProgFitData.InternetAccess)),
                        new InputOutputColumnPair("MotherEduEncoded", nameof(ProgFitData.MotherEducation)),
                        new InputOutputColumnPair("FatherEduEncoded", nameof(ProgFitData.FatherEducation)),
                        new InputOutputColumnPair("TrackEncoded", nameof(ProgFitData.TrackStrand)),
                        new InputOutputColumnPair("GwaEncoded", nameof(ProgFitData.GwaRange)),
                        new InputOutputColumnPair("StudyEncoded", nameof(ProgFitData.StudyHours)),
                        new InputOutputColumnPair("SchoolEncoded", nameof(ProgFitData.SchoolType)),
                        new InputOutputColumnPair("ExtraEncoded", nameof(ProgFitData.Extracurricular)),
                        new InputOutputColumnPair("SubjectEncoded", nameof(ProgFitData.FavoriteSubject)),
                        new InputOutputColumnPair("AwardsEncoded", nameof(ProgFitData.AcademicAwards))
                    }))

                .Append(mlContext.Transforms.NormalizeMinMax("Openness1Norm", nameof(ProgFitData.Openness1)))
                .Append(mlContext.Transforms.NormalizeMinMax("Openness2Norm", nameof(ProgFitData.Openness2)))
                .Append(mlContext.Transforms.NormalizeMinMax("Conscientiousness1Norm", nameof(ProgFitData.Conscientiousness1)))
                .Append(mlContext.Transforms.NormalizeMinMax("Conscientiousness2Norm", nameof(ProgFitData.Conscientiousness2)))
                .Append(mlContext.Transforms.NormalizeMinMax("SelfEfficacy1Norm", nameof(ProgFitData.SelfEfficacy1)))
                .Append(mlContext.Transforms.NormalizeMinMax("SelfEfficacy2Norm", nameof(ProgFitData.SelfEfficacy2)))

                .Append(mlContext.Transforms.Concatenate(
                    "Features",
                    "GenderEncoded",
                    "InterestEncoded",
                    "HobbyEncoded",
                    "IncomeEncoded",
                    "NearSchoolEncoded",
                    "WorkingEncoded",
                    "InternetEncoded",
                    "MotherEduEncoded",
                    "FatherEduEncoded",
                    "TrackEncoded",
                    "GwaEncoded",
                    "StudyEncoded",
                    "SchoolEncoded",
                    "ExtraEncoded",
                    "SubjectEncoded",
                    "AwardsEncoded",
                    "Openness1Norm",
                    "Openness2Norm",
                    "Conscientiousness1Norm",
                    "Conscientiousness2Norm",
                    "SelfEfficacy1Norm",
                    "SelfEfficacy2Norm"))

                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(
                    labelColumnName: "Label",
                    featureColumnName: "Features"))

                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            Console.WriteLine("Evaluating ProgFit model...");

            var cvResults = mlContext.MulticlassClassification.CrossValidate(
                data: trainingData,
                estimator: pipeline,
                numberOfFolds: 5,
                labelColumnName: "Label");

            Console.WriteLine("Cross-validation results:");
            Console.WriteLine($"Average MicroAccuracy: {cvResults.Average(r => r.Metrics.MicroAccuracy):F4}%");
            Console.WriteLine($"Average MacroAccuracy: {cvResults.Average(r => r.Metrics.MacroAccuracy):F4}%");
            Console.WriteLine($"Average LogLoss: {cvResults.Average(r => r.Metrics.LogLoss):F4}%");

            Console.WriteLine("\nTraining final model on full dataset...");
            var model = pipeline.Fit(trainingData);

            var transformedSchema = model.GetOutputSchema(trainingData.Schema);
            var labelNames = default(VBuffer<ReadOnlyMemory<char>>);
            transformedSchema["Score"].Annotations.GetValue("SlotNames", ref labelNames);
            var labels = labelNames.DenseValues().Select(x => x.ToString()).ToArray();

            var predictor = mlContext.Model.CreatePredictionEngine<ProgFitData, ProgFitPrediction>(model);

            var sampleStudent = new ProgFitData
            {
                Gender = "Male",
                PrimaryInterest = "Technology / Computing",
                Hobby = "Coding / Programming",
                IncomeLevel = "Middle income (₱43,828 - ₱76,667 per month)",
                NearSchool = "Yes",
                WorkingStudent = "Yes",
                InternetAccess = "Strong and consistent access",
                MotherEducation = "College Graduate",
                FatherEducation = "College Graduate",
                TrackStrand = "STEM",
                GwaRange = "90 - 94",
                StudyHours = "11-15 hours",
                SchoolType = "Science High School",
                Extracurricular = "Academic Clubs",
                FavoriteSubject = "ICT / Computer Studies",
                AcademicAwards = "No",
                Openness1 = 4,
                Openness2 = 5,
                Conscientiousness1 = 4,
                Conscientiousness2 = 5,
                SelfEfficacy1 = 4,
                SelfEfficacy2 = 4
            };

            var prediction = predictor.Predict(sampleStudent);

            var displayScores = Softmax(prediction.Score);

            //Console.WriteLine("\nPredicted Match Scores");
            //Console.WriteLine("------------------------------");

            var ranked = displayScores
                .Select((p, i) => new { Program = labels[i], Score = p })
                .OrderByDescending(x => x.Score)
                .ToList();

            //foreach (var item in ranked)
            //{
            //    Console.WriteLine($"{item.Program} : {item.Score:P2}");
            //}

            Console.WriteLine("\nRanked Match Scores");
            Console.WriteLine("------------------------------");

            for (int i = 0; i < ranked.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ranked[i].Program} : {(ranked[i].Score * 1000):F4}%");
            }

            Console.WriteLine("\nTop 3 Recommended Programs");
            Console.WriteLine("------------------------------");

            foreach (var item in ranked.Take(3))
            {
                Console.WriteLine($"{item.Program} : {(item.Score * 1000):F4}%");
            }

            Console.WriteLine("------------------------------");
            Console.WriteLine($"Primary Recommendation: {prediction.PredictedMajor}");
            Console.WriteLine("------------------------------");

            Console.ReadLine();
        }

        static double[] Softmax(float[] scores)
        {
            double max = scores.Max();
            double sum = 0;
            double[] probs = new double[scores.Length];

            for (int i = 0; i < scores.Length; i++)
            {
                probs[i] = Math.Exp(scores[i] - max);
                sum += probs[i];
            }

            for (int i = 0; i < probs.Length; i++)
            {
                probs[i] /= sum;
            }

            return probs;
        }
    }
}