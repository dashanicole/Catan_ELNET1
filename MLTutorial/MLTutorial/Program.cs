using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            string dataPath = Path.Combine(AppContext.BaseDirectory, "progfit_dataset.txt"); 

            IDataView data = mlContext.Data.LoadFromTextFile<ProgFitData>(
                path: dataPath,
                hasHeader: true,
                separatorChar: '\t');


            // ================= TRAINERS =================
            var trainers = new (string Name, IEstimator<ITransformer> Trainer)[]
            {
                ("SDCA", mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(
                    labelColumnName: "Label", featureColumnName: "Features")),

                ("LightGBM", mlContext.MulticlassClassification.Trainers.LightGbm(
                    labelColumnName: "Label", featureColumnName: "Features")),

                ("FastTree (OVA)", mlContext.MulticlassClassification.Trainers.OneVersusAll(
                    mlContext.BinaryClassification.Trainers.FastTree(
                        labelColumnName: "Label",
                        featureColumnName: "Features"))),

                ("LbfgsMaxEnt", mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(
                    labelColumnName: "Label", featureColumnName: "Features"))
            };

            // ================= LOOP THROUGH MODELS =================
            foreach (var trainer in trainers)
            {
                Console.WriteLine($"\n==============================");
                Console.WriteLine($"Testing Algorithm: {trainer.Name}");
                Console.WriteLine($"==============================");

                var pipeline =
                    mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(ProgFitData.ChosenMajor))
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(new[]
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
                    .Append(mlContext.Transforms.Concatenate("Features",
                        "GenderEncoded", "InterestEncoded", "HobbyEncoded", "IncomeEncoded",
                        "NearSchoolEncoded", "WorkingEncoded", "InternetEncoded", "MotherEduEncoded",
                        "FatherEduEncoded", "TrackEncoded", "GwaEncoded", "StudyEncoded",
                        "SchoolEncoded", "ExtraEncoded", "SubjectEncoded", "AwardsEncoded",
                        "Openness1Norm", "Openness2Norm", "Conscientiousness1Norm",
                        "Conscientiousness2Norm", "SelfEfficacy1Norm", "SelfEfficacy2Norm"))
                    .Append(trainer.Trainer)
                    .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

                // ================= CROSS VALIDATION =================
                var cvResults = mlContext.MulticlassClassification.CrossValidate(
                    data: data,
                    estimator: pipeline,
                    numberOfFolds: 5,
                    labelColumnName: "Label");

                Console.WriteLine("\nCross-Validation Results:");
                Console.WriteLine($"MicroAccuracy: {cvResults.Average(r => r.Metrics.MicroAccuracy):P2}");
                Console.WriteLine($"MacroAccuracy: {cvResults.Average(r => r.Metrics.MacroAccuracy):P2}");
                Console.WriteLine($"LogLoss: {cvResults.Average(r => r.Metrics.LogLoss):F4}");

                // ================= TRAIN-TEST SPLIT =================
                var split = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);
                var trainSet = split.TrainSet;
                var testSet = split.TestSet;
                
                var model = pipeline.Fit(trainSet);

                // ================= EVALUATION =================
                var predictions = model.Transform(testSet);

                var metrics = mlContext.MulticlassClassification.Evaluate(
                    data: predictions,
                    labelColumnName: "Label",
                    predictedLabelColumnName: "PredictedLabel");

                Console.WriteLine("\n===== FINAL MODEL METRICS =====");
                Console.WriteLine($"Accuracy (Micro): {metrics.MicroAccuracy:P2}");
                Console.WriteLine($"Accuracy (Macro): {metrics.MacroAccuracy:P2}");
                Console.WriteLine($"LogLoss: {metrics.LogLoss:F4}");
                Console.WriteLine($"LogLoss Reduction: {metrics.LogLossReduction:F4}");

                Console.WriteLine("\nConfusion Matrix:");
                Console.WriteLine(metrics.ConfusionMatrix.GetFormattedConfusionTable());

                // ================= PREDICTION =================
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

                Console.WriteLine("\nPrimary Recommendation:");
                Console.WriteLine(prediction.PredictedMajor);
            }

            Console.WriteLine("\nAll models tested successfully!");
        }
    }
}