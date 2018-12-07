def get_first_step(steps)
  # code here
  first_steps = ''
  steps.each_key do |k|
    found = false
    steps.each do |step|
      found = true if step[1].include? k
    end

    first_steps += k if found == false
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

def follow_the_steps(first_steps, steps)
  # code here
  solution = ''
  to_dos = first_steps.chars.sort.join

  while to_dos != ''
    sorted_to_dos = to_dos.chars.sort.join
    sorted_to_dos.chars.each do |t|
      dependencies = get_dependencies(t, steps)

      solution_chars = solution.chars
      dependencies_chars = dependencies.chars

      dep_check_failed = false
      dependencies_chars.each do |d|
        dep_check_failed = true unless solution_chars.include? d
      end

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






puts 'end of code'




