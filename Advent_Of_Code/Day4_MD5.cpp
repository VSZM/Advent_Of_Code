#include <iostream>
#include <cryptopp/md5.h>
#include <cryptopp/hex.h>
#include<string>


int main_day4()
{
	std::string input = "abcdef";
	std::string tmp;

	int num = 609040;
	byte result[32];

	CryptoPP::HexEncoder encoder;


	while (tmp = input + std::to_string(num), CryptoPP::MD5().CalculateDigest(result, (byte*)tmp.c_str(), tmp.length()),
		!(tmp[0] == 0 && tmp[1] == 0 && tmp[2] && tmp[3] == 0 && tmp[4] == 0))
	{
		std::cout << num << result << std::endl;
		++num;
	}

	std::cout << num << std::endl;

	return 0;
}

