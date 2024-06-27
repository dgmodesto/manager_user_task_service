import HeaderContent from '@/components/HeaderContent';
import NewTaskForm from '@/components/NewTaskForm';
import { getServerSession } from 'next-auth';
import { redirect } from 'next/navigation';
import { Suspense } from 'react';

export default async function NewTask() {
  const session = await getServerSession();

  if (!session) {
    redirect('/')
  }

  return (
    <div>
      <Suspense fallback={<div>Loading...</div>}>
        <HeaderContent />
        <NewTaskForm />
      </Suspense>

    </div>
  )
}