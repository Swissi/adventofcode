def get_first_step(steps)
  # code here
  first_steps = ''
  steps.each_key do |k|
    found = false
    steps.each do |step|
      found = true if step[1].include? k
    end

    first_steps += k unless found
  end

  first_steps
end

def get_dependencies(step, steps)
  # code here
  #
  dep = ''

  steps.each do |s|
    dep += s[0] if s[1].include? step
  end

  dep
end

def check_dependencies(solution, steps, t)
  dependencies = get_dependencies(t, steps)

  solution_chars = solution.chars
  dependencies_chars = dependencies.chars

  return false if solution_chars == dependencies_chars

  dep_check_failed = false
  dependencies_chars.each do |d|
    dep_check_failed = true unless solution_chars.include? d
  end
  dep_check_failed
end

def follow_the_steps(first_steps, steps)
  # code here
  solution = ''
  to_dos = first_steps.chars.sort.join

  while to_dos != ''
    sorted_to_dos = to_dos.chars.sort.join
    sorted_to_dos.chars.each do |t|
      dep_check_failed = check_dependencies(solution, steps, t)

      next if dep_check_failed

      solution += t
      x = steps[t]
      # BD, ""

      to_dos = to_dos.sub(t, '')
      if x.nil?
        # has no key in steps
      else
        x_chars = x.chars

        x_chars.each do |xc|
          if to_dos.chars.include? xc
            next
          else
            to_dos += xc
          end
        end
      end

      break
    end
  end

  solution
end


##
# Step C must be finished before step A can begin.
# Step C must be finished before step F can begin.
# Step A must be finished before step B can begin.
# Step A must be finished before step D can begin.
# Step B must be finished before step E can begin.
# Step D must be finished before step E can begin.
# Step F must be finished before step E can begin.
#
lines = File.readlines('i.txt')
steps = {}

lines.each do |line|
  arr = line.split(' ')
  parent = arr[1]
  child = arr[7]

  if steps[parent].nil?
    steps[parent] = child
  else
    steps[parent] += child
  end
end

# get key which never is in a value
first_steps = get_first_step(steps)
puts 'first steps: ' + first_steps

solution = follow_the_steps(first_steps, steps)
puts solution


open = solution
done = ''
workers = {}
workers_count = 5
seconds = 3600
total_seconds = 0

def get_open_task(open, done, steps)
  open.chars.each do |c|
    return c unless check_dependencies(done, steps, c)
  end

  nil
end

(1..workers_count).each do |i|
  workers[i] = 'idle'
end

def get_second(x)
  x.ord - 'A'.ord + 1
end

(0..seconds).each do |s|
  workers.each_key do |k|
    unless workers[k] == 'idle'
      workers[k][1] > 0
      workers[k][1] -= 1
    end

    if workers[k][1] == 0
      done += workers[k][0]
      workers[k] = 'idle'
    end
  end


  print s.to_s + "\t"
  workers.each_key do |k|
    next unless workers[k] == 'idle'

    x = get_open_task(open, done, steps)

    break if x.nil?

    workers[k] = [x, get_second(x) + 60]
    open = open.sub(x, '')

  end

  workers.each_key do |k|
    if workers[k] == 'idle'
      print ".\t\t"
    else
      print workers[k].to_s + "\t"
    end
  end
  print done
  puts

  if done.length == solution.length
    total_seconds = s
    break
  end
end

puts 'total seconds needed: ' + total_seconds.to_s
puts 'end of code'




