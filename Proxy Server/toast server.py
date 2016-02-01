from socket import *

sock = socket()
sock.bind(('0.0.0.0', 9001))
sock.listen(10)
proxy, addr = sock.accept()
print proxy.recv(1024)