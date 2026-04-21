const { Server } = require("@modelcontextprotocol/sdk/server/index.js");
const { StdioServerTransport } = require("@modelcontextprotocol/sdk/server/stdio.js");
const { CallToolRequestSchema, ListToolsRequestSchema } = require("@modelcontextprotocol/sdk/types.js");
const { z } = require("zod");

// MCP Szerver inicializálása
const server = new Server(
  {
    name: "library-mcp-server",
    version: "1.0.0",
  },
  {
    capabilities: {
      tools: {},
    },
  }
);

// 1. Definiálja az AI számára elérhető eszközöket (Tools)
server.setRequestHandler(ListToolsRequestSchema, async () => {
  return {
    tools: [
      {
        name: "add_book",
        description: "Új könyv hozzáadása a Kubernetes alapú könyvtárhoz",
        inputSchema: {
          type: "object",
          properties: {
            title: { type: "string", description: "A könyv címe" },
            author: { type: "string", description: "A könyv szerzője" },
            year: { type: "number", description: "Kiadási év" },
          },
          required: ["title", "author", "year"],
        },
      },
    ],
  };
});

// 2. mi történjen, ha az AI meghívja az "add_book" eszközt
server.setRequestHandler(CallToolRequestSchema, async (request) => {
  if (request.params.name === "add_book") {
    const { title, author, year } = request.params.arguments;

    try {
      // Itt hívható meg a futó Backend API, amit port-forwardolva van az 5000-es porton
      const response = await fetch("http://localhost:5000/api/books", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ title, author, year }),
      });

      if (!response.ok) throw new Error(`API hiba: ${response.status}`);

      return {
        content: [{ type: "text", text: `Sikeresen hozzáadva: ${title} (${author}, ${year})` }],
      };
    } catch (error) {
      return {
        content: [{ type: "text", text: `Hiba történt: ${error.message}` }],
        isError: true,
      };
    }
  }
  throw new Error("Ismeretlen eszköz");
});

// Szerver indítása
async function main() {
  const transport = new StdioServerTransport();
  await server.connect(transport);
  console.error("Library MCP Server fut...");
}

main().catch(console.error);