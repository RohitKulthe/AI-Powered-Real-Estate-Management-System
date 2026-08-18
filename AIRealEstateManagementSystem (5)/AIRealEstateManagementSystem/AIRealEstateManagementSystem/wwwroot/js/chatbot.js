document.addEventListener("DOMContentLoaded", function () {

    const toggleButton = document.getElementById("chatbot-toggle");
    const chatWindow = document.getElementById("chatbot-window");
    const sendButton = document.getElementById("sendMessage");
    const userInput = document.getElementById("userMessage");
    const chatBody = document.getElementById("chat-body");

    // Check if chatbot elements exist
    if (!toggleButton || !chatWindow || !sendButton || !userInput || !chatBody) {
        console.error("Chatbot elements not found.");
        return;
    }

    // Open / Close Chatbot
    toggleButton.addEventListener("click", function () {

        if (chatWindow.style.display === "none" || chatWindow.style.display === "") {
            chatWindow.style.display = "block";
        } else {
            chatWindow.style.display = "none";
        }

    });

    // Send Button
    sendButton.addEventListener("click", sendMessage);

    // Send on Enter Key
    userInput.addEventListener("keypress", function (e) {

        if (e.key === "Enter") {
            e.preventDefault();
            sendMessage();
        }

    });

    function sendMessage() {

        const message = userInput.value.trim();

        if (message === "")
            return;

        // Show User Message
        chatBody.innerHTML += `
            <div class="user-message">
                ${message}
            </div>
        `;

        chatBody.scrollTop = chatBody.scrollHeight;
        userInput.value = "";

        // Send message to server
        fetch("/Chat/SendMessage", {

            method: "POST",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify({
                message: message
            })

        })
            .then(response => {

                if (!response.ok) {
                    throw new Error("Server Error");
                }

                return response.json();

            })
            .then(data => {

                chatBody.innerHTML += `
                <div class="bot-message">
                    ${data.response}
                </div>
            `;

                chatBody.scrollTop = chatBody.scrollHeight;

            })
            .catch(error => {

                console.error(error);

                chatBody.innerHTML += `
                <div class="bot-message">
                    ❌ Unable to connect to the AI Assistant.
                </div>
            `;

                chatBody.scrollTop = chatBody.scrollHeight;

            });

    }

});