# Bitcoin Puzzle #67 Solver

This project is a high-performance solver for the Bitcoin Puzzle #67. It searches for the private key that matches a specific target Bitcoin address, using parallel processing to maximize key-checking speed. If it finds the private key, it can optionally send a WhatsApp message notification.

## How It Works

The solver generates random private keys within a specific range and checks if they match the target Bitcoin address (`1BY8GQbnueYofwSuFAT3USAhGjPrkxDdW9`). If a match is found, the private key details are displayed on the console. Optionally, if the user configures WhatsApp messaging, the private key information will be sent to a specified phone number via [Whatabot](https://whatabot.io/get-started).

### Docker Setup

To run this solver in a Docker container, follow these steps:

1. **Clone the Repository** (if needed):
   ```bash
   git clone https://github.com/bitpuzz/1000BtcChallenge.git
   cd 1000BtcChallenge
   ```
2. **Build the Docker Image**:
   ```bash
   docker build -t btc-puzzle-solver .
   ```
3. **Run the Docker Container (Change the CPUs as needed)**:
    #### With Whatsapp Messaging Sending Disabled  
   ```bash
   docker run --cpus="4" btc-puzzle-solver
   ```
    #### With Whatsapp Messaging Sending Enabled
   ```bash
   docker run --cpus="4" -e API_KEY="your-api-key" -e PHONE="your-phone-number" -e ENABLE_MESSAGE_SENDING=true btc-puzzle-solver
   ```
## Additional Information

- Performance Tracking: The program outputs the number of keys generated per second.
- Logging: Key search information, including any matched private keys, will be printed to the console.

## Important Notice

This tool should be used responsibly. Bitcoin puzzles are challenging cryptographic puzzles, and attempting to solve them may require significant computational power.

## Support & Collaboration
If you'd like to support the project or collaborate, feel free to contribute by donating to the following Bitcoin address:

   #### Bitcoin Address for Donations:
   ```
    bc1qwlgv4dfc2083nhs5a6z7k0pmf0nqfnjqa24esy
   ```

### Keywords
- Bitcoin Puzzle #67 Solver
- Bitcoin Puzzle Solver
- Bitcoin private key generator
- Bitcoin address checker
- Dockerized Bitcoin key searcher
- Send WhatsApp notifications with Bitcoin keys