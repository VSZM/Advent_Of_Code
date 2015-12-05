#include <iostream>
#include <cryptopp/md5.h>
#include<string>


bool starts_with_5_zeroes_in_hex(byte b[])
{
	return b[0] == 0 && b[1] == 0 && b[2] < 16;
}

bool starts_with_6_zeroes_in_hex(byte b[])
{
	return b[0] == 0 && b[1] == 0 && b[2] == 0;
}

int main()
{
	std::string input = "ckczppom";
	std::string tmp;

	int num = 1;
	byte result[CryptoPP::MD5::DIGESTSIZE];

	
	while (tmp = input + std::to_string(num), CryptoPP::MD5().CalculateDigest(result, (byte*)tmp.c_str(), tmp.length()),
		!(starts_with_6_zeroes_in_hex(result)))
	{
		++num;
	}

	std::cout << num << std::endl;

	return 0;
}

