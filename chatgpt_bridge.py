from flask import Flask, request, jsonify
import requests

app = Flask(__name__)

OPENAI_API_KEY = "YOUR_API_KEY"
HEADERS = {
    "Authorization": f"Bearer {OPENAI_API_KEY}"
}
API_URL = "https://api.openai.com/v1/chat/completions"

@app.route('/chatgpt', methods=['POST'])
def chatgpt():
    data = request.json
    query = data.get("query")

    if not query:
        return jsonify({"error": "No query provided"}), 400

    response = requests.post(
        API_URL,
        headers=HEADERS,
        json={
            "model": "gpt-3.5-turbo",
            "messages": [{"role": "system", "content": "You're an Ultima Online NPC. Your goal is to answer players questions about the game. Short Answers."},
                         {"role": "user", "content": f"{query}"}],
        }
    )

    if response.status_code == 200:
        content = response.json()
        return jsonify({"response": content['choices'][0]['message']['content']})
    else:
        return jsonify({"error": "Failed to get response from OpenAI"}), response.status_code

if __name__ == '__main__':
    app.run(debug=True, port=5000)
