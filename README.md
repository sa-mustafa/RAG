# RAG with Microsoft.KernelMemory and Ollama

This project demonstrates how to build a **Retrieval-Augmented Generation (RAG)** pipeline in **.NET 8** using **Microsoft.KernelMemory** and **Ollama** as the local AI engine.

It loads a Markdown file, embeds it, stores it in an in-memory vector database, and answers user queries based on the embedded content.

---

## 🚀 Features

- **Local AI processing** via [Ollama](https://ollama.ai/)
- **Text embedding** and **generation** using customizable models
- **Simple vector database** for document search (FAISS-like in-memory)
- **Fast RAG queries** using Microsoft.KernelMemory abstractions
- **No cloud dependencies** — fully local and private

---

## 🧩 Tech Stack

- **.NET 8**
- **Microsoft.KernelMemory**
- **Ollama** (local LLM runtime)
- **Llama 3.1** for generation
- **BGE-M3** for embedding

---

## 🛠️ Prerequisites

1. **Install Ollama**  
   [Download and install Ollama](https://ollama.ai/download) for your OS.

2. **Pull required models**  
   Run the following commands in your terminal:
   ```bash
   ollama pull llama3.1
   ollama pull bge-m3
