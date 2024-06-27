import { getServerSession } from 'next-auth';
import { redirect } from 'next/navigation';
import LogoutButton from './LogoutButton';

export default async function HeaderContent() {
  const session = await getServerSession();

  if (!session) {
    redirect('/');
  }
  return (
    <div className='w-full flex justify-around items-centerflex p-4 mb-5 bg-slate-100'>
      <div>Hello, {session?.user?.name}</div>
      <h2 className='text-bold text-lg'>{'Manager Task User'}</h2>
      <div><LogoutButton /></div>
    </div>
  )
}