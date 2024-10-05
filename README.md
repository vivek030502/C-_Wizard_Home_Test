#TCP Client-Server Packet Transmission with Missing Sequence Handling

##Overview

This project implements a TCP client that connects to a server to receive packets containing trading information. The client can detect missing packets based on sequence numbers and requests the server to resend any missing packets. The packets received are parsed and saved to a JSON file for further analysis.
---

##Table of Contents

Requirements
Project Structure
How It Works
Running the Application Locally
Images
Output
Contributing
License
---

##Requirements

.NET SDK (version 5.0 or later)
Newtonsoft.Json package for JSON serialization
A TCP server to respond to the client's requests (see server implementation section)
---

##Project Structure
arduino
Copy code
/Server-Client_Packet_Transmission
├── ABXExchangeClient
│   └── ABXExchangeClient
|   |  └── Program.cs
|   |  └── bin
|   |      └── output,json
├── abx_exchange_server
│   └── main.js
---

##How It Works

Client Connection: The client connects to a specified TCP server.
Packet Reading: The client sends a request to the server and begins reading packets from the stream.
Packet Parsing: Each packet is parsed to extract the symbol, buy/sell indicator, quantity, price, and sequence number.
Missing Sequence Detection: The client tracks received sequence numbers and identifies any missing packets.
Resend Requests: For each missing sequence, the client requests the server to resend the corresponding packet.
JSON Output: All received packets are saved to an output.json file for analysis.
---

##Running the Application Locally
To run this application on your local machine, follow these steps:

###Install .NET SDK:

Download and install the .NET SDK from the .NET Download page.

###Clone the Repository:

bash
Copy code
git clone <repository-url>
cd PacketTransmission

###Install Dependencies: Make sure to restore the NuGet packages required for the project. Run the following command in the project directory:

bash
Copy code
dotnet restore

###Run the Server: Before starting the client, ensure the server is running. Use the provided server code or your own implementation.
bash
Copy code
node main.js

###Run the Client: Open a new terminal, navigate to the Client directory, and run the following command:

bash
Copy code
dotnet run

### OR 
Open the ABXExchange Client folderon Visual Studio and run the Program.cs file by clicking Start button on navigation menu.

###View the Output: After execution, check the output.json file generated in the bin folder for the received packets.
---

##Output
The output.json file contains the parsed packet data in a structured format. Below is an example of the expected output:

json
Copy code
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
---

##Contributing
Contributions are welcome! If you would like to contribute, please follow these steps:

Fork the repository.
Create a new branch for your feature or fix.
Commit your changes and push to your branch.
Submit a pull request.
---

##License
This project is licensed under the MIT License. See the LICENSE file for details.
