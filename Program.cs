using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI.Ollama;
using Microsoft.KernelMemory.Configuration;
using System.Diagnostics;

namespace rag
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var ollamaUri = "http://127.0.0.1:11434";
            // Right
            //var textModel = "gemma3:latest";   // generator, 128K
            //var embedModel = "nomic-embed-text:latest";    // embedder, 2K
            // Partially right
            //var textModel = "gemma3:latest";   // generator, 128K
            //var embedModel = "bge-m3:latest";    // embedder, 8K
            // Partially right
            //var textModel = "gemma3:latest";   // generator, 128K
            //var embedModel = "embeddinggemma:latest";    // embedder, 2K
            // Right, with Thinking Process
            //var textModel = "qwen3:latest";   // generator, 40K
            //var embedModel = "embeddinggemma:latest";    // embedder, 2K
            // Wrong
            //var textModel = "qwen3:latest";   // generator, 40K
            //var embedModel = "qwen3-embedding:8b";    // embedder, 40K
            // Right
            //var textModel = "llama3.1:latest";   // generator, 128K
            //var embedModel = "nomic-embed-text:latest";    // embedder, 2K
            // Right
            var textModel = "llama3.1:latest";   // generator, 128K
            var embedModel = "bge-m3:latest";    // embedder, 8K

            var config = new OllamaConfig
            {
                Endpoint = ollamaUri,
                TextModel = new OllamaModelConfig(textModel, 128 * 1024),
                EmbeddingModel = new OllamaModelConfig(embedModel, 8 * 1024)
            };

            var chunking = new TextPartitioningOptions
            {
                //(~20% overlap)
                OverlappingTokens = 100,
                MaxTokensPerParagraph = 500,
            };

            // Build KM pipeline
            var memory = new KernelMemoryBuilder()
                .WithOllamaTextGeneration(config)
                .WithOllamaTextEmbeddingGeneration(config)
                //.WithCustomTextPartitioningOptions(chunking)
                .WithSearchClientConfig(new SearchClientConfig
                {
                    EmptyAnswer = "Nothing Found",
                    MaxMatchesCount = 8, // Top-k neighbors
                    AnswerTokens = 512,  // max length of LLM answer
                })
                .WithSimpleVectorDb() // in-memory FAISS-like
                .Build<MemoryServerless>();

            // 1. Load Markdown file
            string mdPath = @"C:/Users/User/source/repos/rag/bin/Debug/net8.0/roya.md";

            // 2. Clean Markdown → plain text
            // KernelMemory has Markdown extractor, but you can also strip formatting manually
            string plainText = File.ReadAllText(mdPath);

            // 3. Import into KM
            await memory.ImportTextAsync(
                text: plainText,
                documentId: "doc1");

            Console.WriteLine("Document ingested.");

            // 4. Ask a query (embed only the user’s query text)
            var query = "ارتفاع تشک مدیکال 1 چقدر هست؟";
            //var query = "ارتفاع تشک مدیکال پلاس چقدر هست؟";
            var timer = Stopwatch.StartNew();
            var answer = await memory.AskAsync(query);
            timer.Stop();

            Console.WriteLine($"=== ANSWER ({timer.ElapsedMilliseconds} ms) ===");
            Console.WriteLine(answer.Result);
            Console.WriteLine("=== SOURCES ===");
            foreach (var s in answer.RelevantSources)
            {
                Console.WriteLine($"- {s.SourceName} (chunk {s.DocumentId})");
            }
        }
    }
}
