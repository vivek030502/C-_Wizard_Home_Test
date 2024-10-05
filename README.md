# TCP Client-Server Packet Transmission with Missing Sequence Handling

## Overview

This project implements a TCP client that connects to a server to receive packets containing trading information. The client can detect missing packets based on sequence numbers and requests the server to resend any missing packets. The packets received are parsed and saved to a JSON file for further analysis.

## Table of Contents

- [Requirements](#requirements)
- [Project Structure](#project-structure)
- [How It Works](#how-it-works)
- [Running the Application Locally](#running-the-application-locally)
- [Output](#output)
- [Contributing](#contributing)
- [License](#license)

## Requirements

- .NET SDK (version 5.0 or later)
- Newtonsoft.Json package for JSON serialization
- A TCP server to respond to the client's requests (see server implementation section)

## Project Structure

```
/Server-Client_Packet_Transmission
├── ABXExchangeClient
│   ├── Program.cs
│   └── bin
│       └── output.json
└── abx_exchange_server
    └── main.js
```

## How It Works

1. **Client Connection**: The client connects to a specified TCP server.
2. **Packet Reading**: The client sends a request to the server and begins reading packets from the stream.
3. **Packet Parsing**: Each packet is parsed to extract the symbol, buy/sell indicator, quantity, price, and sequence number.
4. **Missing Sequence Detection**: The client tracks received sequence numbers and identifies any missing packets.
5. **Resend Requests**: For each missing sequence, the client requests the server to resend the corresponding packet.
6. **JSON Output**: All received packets are saved to an `output.json` file for analysis.

## Running the Application Locally

To run this application on your local machine, follow these steps:

### Install .NET SDK

Download and install the .NET SDK from the [.NET Download page](https://dotnet.microsoft.com/download/dotnet).

### Clone the Repository

```bash
git clone <repository-url>
cd Server-Client_Packet_Transmission
```

### Install Dependencies

Make sure to restore the NuGet packages required for the project. Run the following command in the project directory:

```bash
dotnet restore
```

### Run the Server

Before starting the client, ensure the server is running. Open Command Prompt or any terminal and change the directory to abx_exchange_server and run the following code:

```bash
node main.js
```

### Run the Client

Open a new terminal, navigate to the Client directory, and run the following command:

```bash
dotnet run
```

**OR**

Open the `ABXExchangeClient` folder in Visual Studio and run the `Program.cs` file by clicking the Start button in the navigation menu.

### View the Output

After execution, check the `output.json` file generated in the `bin` folder for the received packets.

## Output

The `output.json` file contains the parsed packet data in a structured format. Below is an example of the expected output:

```json
[
  {
    "Symbol": "AAPL",
    "BuySell": "B",
    "Quantity": 100,
    "Price": 150,
    "Sequence": 1
  },
  {
    "Symbol": "AAPL",
    "BuySell": "S",
    "Quantity": 50,
    "Price": 155,
    "Sequence": 2
  }
]
```

## Contributing

Contributions are welcome! If you would like to contribute, please follow these steps:

1. Fork the repository.
2. Create a new branch for your feature or fix.
3. Commit your changes and push to your branch.
4. Submit a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
