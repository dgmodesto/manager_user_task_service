import NextAuth from 'next-auth/next';
import CredentialsProvider from 'next-auth/providers/credentials';
import { cookies } from "next/headers";

const handler = NextAuth({
  pages: {
    signIn: '/',
    error: '/'
  },
  providers: [
    CredentialsProvider({

      name: 'Credentials',
      credentials: {
        username: { label: "Username", type: "text", placeholder: "jsmith" },
        password: { label: "Password", type: "password" }
      },
      async authorize(credentials, req) {
        console.log('env_url: ' + process.env.API_AUTH_URL);

        const res = await fetch(`${process.env.API_AUTH_URL}/Auth/login`, {
          method: 'POST',
          body: JSON.stringify(credentials),
          headers: { "Content-Type": "application/json" }
        })
        const user = await res.json()

        cookies().set("token-jwt", user.data.access_token);

        if (res.ok && user) {
          return user
        }
        return null
      }
    })
  ],
  secret: process.env.NEXTAUTH_SECRET,
})

export { handler as GET, handler as POST };

