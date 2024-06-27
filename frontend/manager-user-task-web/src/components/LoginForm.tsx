'use client'

import { signIn } from 'next-auth/react'
import { useSearchParams } from 'next/navigation';

export default function LoginForm() {
  const searchParams = useSearchParams();

  const error = searchParams.get('error');


  async function login(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const formData = new FormData(e.currentTarget)

    const data = {
      username: formData.get('username'),
      password: formData.get('password')
    }

    signIn('credentials', {
      ...data,
      callbackUrl: '/task'
    })

  }

  return (
    <form
      onSubmit={login}
      className='bg-white p-12 rounded-lg w-96 max-w-full flex justify-center items-center flex-col gap-2'
    >

      <h2 className='font-bold text-xl mb-3'>Sign In</h2>

      <input
        name='username'
        type="text"
        placeholder='username'
        className='input input-primary  w-full' />

      <input
        name='password'
        type="password"
        placeholder='password'
        className='input input-primary  w-full' />

      <button className='btn btn-primary w-full' type='submit'>Login</button>
      {error?.includes('Unexpected') && <div className='text-red-800'>Invalid credentials</div>}
    </form>)
}