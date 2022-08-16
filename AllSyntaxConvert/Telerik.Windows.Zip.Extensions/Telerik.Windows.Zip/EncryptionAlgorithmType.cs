using System;

namespace Telerik.Windows.Zip
{
	enum EncryptionAlgorithmType
	{
		Unknown = -1,
		Des = 26113,
		Rc2Old,
		TripleDes168,
		TripleDes112 = 26121,
		Aes128 = 26126,
		Aes192,
		Aes256,
		Rc2 = 26370,
		Blowfish = 26400,
		Twofish,
		Rc4 = 26625
	}
}
