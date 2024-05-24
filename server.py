import socket

HOST = "95.165.135.33"
PORT = 7777

server = socket.socket()
server.bind((HOST, PORT))
server.listen(5)