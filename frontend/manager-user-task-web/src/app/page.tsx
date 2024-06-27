import LoginForm from '@/components/LoginForm';
import { getServerSession } from 'next-auth';
import Image from "next/image";
import { redirect } from 'next/navigation';
import { Suspense } from 'react';

export default async function Home() {

  return (
    <main>
      <div className='h-screen flex justify-center items-center bg-slate-600 px-5'>

        <Suspense fallback={<div>Loading...</div>}>
          <LoginForm />
        </Suspense>
      </div>
    </main>
  );
}
