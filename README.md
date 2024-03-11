# Ultima Online ChatGPT NPC Integration

This proof of concept integrates OpenAI's ChatGPT with an NPC in the Ultima Online server environment using ServUO. It allows players to query ChatGPT using an NPC OnSpeech override to query and response display.


![image](https://github.com/hilario-fn/ultima_online_npc_gpt/assets/58054675/f5d13682-a018-4a77-ae4b-dd15c02d86b3)

## System Architecture Diagram

![UO GPT drawio](https://github.com/hilario-fn/ultima_online_npc_gpt/assets/58054675/ed4bbb34-f673-4e12-b626-82918cb546cc)

## Requirements

- ServUO server environment set up and running.
- An active OpenAI API key with access to the GPT-3.5-turbo model.
- Python 3
- Flask for running the intermediary API service. (chatgpt_bridge.py)
- Mono for running the ServUO on non-Windows systems.

## Files

- /ServUO/Scripts/Mobiles/NPCsSimpleChatVendor.cs: Contains the ChatGPT NPC script. 
- /ServUO/Scripts/Custom/chatgpt_bridge.py: Manages OpenAI Requests. 

## Setup and Installation

1. **ServUO Configuration**: Place the `SimpleChatVendor.cs` script in the `Scripts/Mobiles/NPCs` directory, set `PythonPath` and `ChatScriptPath` variables.

2. **Flask Service**: Set up the Flask service by running the `chatgpt_bridge.py` script. This script should be hosted on the same server as ServUO or made accessible to the ServUO server. 

   ![image](https://github.com/hilario-fn/ultima_online_npc_gpt/assets/58054675/ba175277-6e68-45a5-b6c1-c078092814d8)

3. **API Key**: Replace the placeholder `YOUR_API_KEY` in `chatgpt_bridge.py` with your actual OpenAI API key.

4. **Newtonsoft.Json Installation**: Use `gacutil` to install the `Newtonsoft.Json.dll` into the Global Assembly Cache for Mono.

   (You can use the Newtonsoft.Json.dll uploaded in this project (donet45 version) or you can Download it by yourself from the Json.NET website)

5. **Download Newtonsoft library**: https://www.newtonsoft.com/json (if you rather, I uploaded the dll to this project already.
- Extract the contents to a folder of preference
- Navigate to the folder containing the dll. **For this project I used the dll in the "dotnet45" folder**
- Run the following command:
      ```
      gacutil install Newtonsoft.Json.dll
      ```

6. Use the following command to find out the dll path. 
```
   find /usr/lib/mono/gac | grep Newtonsoft.Json.dll
```

7. Copy Newtonsoft.Json.dll to /opt/ServUO/Newtonsoft/ (Create the folder if you haven't already) 

8. Add the following entries to ServUO/Scripts/Scripts.csproj:
   
   ```
   <ItemGroup>
   <Reference Include="Newtonsoft.Json">
    <HintPath>/opt/ServUO/Newtonsoft/Newtonsoft.Json.dll</HintPath>
   </Reference>
   </ItemGroup>
   <ItemGroup>
    <Reference Include="System.Net.Http" />
   </ItemGroup>
   ``` 

9. Add the following entries to Data/Assemblies.cfg

```
/opt/ServUO/Newtonsoft/Newtonsoft.Json.dll
System.Net.Http
```

10. Recompile ServUO

```
cd /opt/ServUO/
make
```


   
## Usage

1. Add a SimpleChatVendor NPC
2. Approach the NPC
3. Initiate a conversation with the "chat" command followed by your query. For example, "chat, when was Ultima Online released?" and the NPC will respond.

## My Setup (For reference only)

- Raspberry PI 4 - arm64 
- Raspbian (bullseye)
- ServUO Pub57
- Mono  6.12.0.200
- Net Framework 4.5
- Python 3.9.2



