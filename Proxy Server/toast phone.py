from socket import *
from Encryptor import *
from Crypto.Cipher import AES

encryptor = Encryptor(AES.new(b'01234567890123456789012345678901'))
sock = socket()
sock.connect(('127.0.0.1', 9000))
sock.send(encryptor.encrypt('1234567890123456'))