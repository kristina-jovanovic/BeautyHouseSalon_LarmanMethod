﻿using Common.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.SystemOperations
{
    internal class SOUlogujKorisnika : SOBase
    {
        private Korisnik k; //korisnik koji pokusava da se prijavi
        public IEntity result; //prijavljeni korisnik
        public bool pogresnaLozinka = false;
        public SOUlogujKorisnika(Korisnik k)
        {
            this.k = k;
        }
        protected override void ExecuteConcreteOperation()
        {
            result = broker.GetEntityById(k);
            if(result != null)
            {
                Korisnik dobijeniKorisnik = (Korisnik)result;
                if (!ProveriLozinku(dobijeniKorisnik, k))
                {
                    pogresnaLozinka = true;
                    return;
                }
            }

        }
        private bool ProveriLozinku(Korisnik dobijeniKorisnik, Korisnik k)
        {
            byte[] salt = Convert.FromBase64String(dobijeniKorisnik.KodZaEnkripciju);
            string hashedPassword = HashFunction.HashPassword(k.Lozinka, salt);
            //Debug.WriteLine($"===========SALT: {dobijeniKorisnik.KodZaEnkripciju}, PASS: {dobijeniKorisnik.Lozinka}, UNETA L:{k.Lozinka}");

            if (dobijeniKorisnik.Lozinka != hashedPassword)
            {
                return false; //pogresno uneta lozinka za taj username
            }
            return true;
        }
    }
}
