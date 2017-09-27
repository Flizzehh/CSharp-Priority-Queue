using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueue {
	class Program {
		class PriorityQueue<T> {
			private List<PriorityElement> pQ = new List<PriorityElement>();
			private int _count = 0;

			public int Count {
				get { return _count; }
			}

			public enum Order { Ascending, Descending };

			class PriorityElement {
				public T element;
				public int priority;
				public PriorityElement(object element, int priority) {
					this.element = (T)element;
					this.priority = priority;
				}
			}

			public void Enqueue(T element,int priority, Order order) {
				if (pQ.Count > 0) {
					int left = 0, right = pQ.Count, mid = 0;
					if (order == Order.Descending) {
						while (left < right) {
							mid = (int)Math.Floor((double)(left + (right - left) / 2));
							if (pQ[mid].priority < priority) { // less than, largest first
								right = mid;
							} else {
								left = mid + 1;
							}
						}
					} else if (order == Order.Ascending) {
						while (left < right) {
							mid = (int)Math.Floor((double)(left + (right - left) / 2));
							if (pQ[mid].priority > priority) { // greater than, smallest first
								right = mid;
							} else {
								left = mid + 1;
							}
						}
					}
					pQ.Insert(right,new PriorityElement(element,priority));
				} else {
					pQ.Add(new PriorityElement(element,priority));
				}
				_count += 1;
			}

			public T Dequeue() {
				if (pQ.Count > 0) {
					T element = pQ[0].element;
					pQ.RemoveAt(0);
					_count -= 1;
					return element;
				} else {
					return default(T);
				}
			}

			public void PrioritizeExistingQueue<SortType>(Dictionary<SortType,int> priorities, List<T> existingList, string property, Order order) {
				foreach (T item in existingList) {
					Enqueue(item,priorities[(SortType)(typeof(T).GetProperty(property).GetValue(item))],order);
				}
			}

			public void Print() {
				string output = "";
				for (int i = 0;i < _count;i++) {
					output += pQ[i].element + ":" + pQ[i].priority + " ";
				}
				Console.WriteLine(output);
			}

			public void PrintAsType<PrintType>(string property) {
				string output = "";
				for (int i = 0;i < _count;i++) {
					output += (PrintType)(typeof(T).GetProperty(property).GetValue(pQ[i].element)) + ":" + pQ[i].priority + " ";
				}
				Console.WriteLine(output);
			}
		}

		enum Jobs { Labour, Building, Farming, Research };

		class Job {
			public Jobs jobType { get; set; }
			public int location { get; set; }
			public Job(Jobs jobType,int location) {
				this.jobType = jobType;
				this.location = location;
			}
		}

		static void Main(string[] args) {

			Dictionary<Jobs,int> priorities = new Dictionary<Jobs,int>() { { Jobs.Labour,1 },{ Jobs.Farming,2 },{ Jobs.Building,3 },{ Jobs.Research,4 } };
			List<Job> jobs = new List<Job>();

			Random randomNumber = new Random();

			for (int i = 0;i < 10000;i++) {
				jobs.Add(new Job((Jobs)(randomNumber.Next() % 4 + 1)-1,i));
				Console.WriteLine(jobs[i].jobType.ToString() + ":" + jobs[i].location);
			}
			Console.WriteLine("Finished creating initial list");

			//Console.ReadKey();

			PriorityQueue<Job> pQ = new PriorityQueue<Job>();
			pQ.PrioritizeExistingQueue(priorities,jobs,nameof(Job.jobType),PriorityQueue<Job>.Order.Ascending);
			Console.WriteLine("Finished prioritizing existing queue");
			pQ.PrintAsType<Jobs>(nameof(Job.jobType));
			pQ.PrintAsType<int>(nameof(Job.location));

			Job job = pQ.Dequeue();
			Console.WriteLine(job.jobType + " " + job.location);

			Console.ReadKey();

			/*
			Random randomNumber = new Random();
			PriorityQueue<int> queue = new PriorityQueue<int>();
			for (int i = 0;i < 1000;i++) {
				int newPriority = randomNumber.Next() % 100 + 1;
				queue.Enqueue(i,newPriority);
			}
			queue.Print();
			Console.ReadKey();
			*/
		}
	}
}
